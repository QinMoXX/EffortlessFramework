using System;
using System.Threading;
using AOT.Framework.ObjectPool;
using AOT.Framework.Resource;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AOT.Framework.Audio
{
    public class AudioObject:ObjectBase,IAudioHandle
    {
        public static Transform AudioRoot;
        private const string AUDIO_PR = "[Audio]";
        private AudioSource m_audioSource;
        private CancellationTokenSource cts;
        internal Guid m_guid;
        
        /// <summary>
        /// 获取音频对象的唯一标识符。
        /// </summary>
        public Guid Guid
        {
            get => m_guid;
        }
        /// <summary>
        /// 初始化音频对象。
        /// </summary>
        /// <param name="name">音频名称。</param>
        protected internal override void Initialize(string name)
        {
            base.Initialize(name);
            AudioClip audioClip = GameEntry.GetModule<ResourceManager>().LoadAssetSync<AudioClip>(name);
            GameObject audioObject = new GameObject(AUDIO_PR+name);
            audioObject.transform.position = new Vector3();
            if (AudioRoot)audioObject.transform.SetParent(AudioRoot,false);
            m_audioSource = audioObject.AddComponent<AudioSource>();
            m_audioSource.loop = false;
            m_audioSource.playOnAwake = false;
            m_audioSource.clip = audioClip;
            Target = audioObject;
            cts = new CancellationTokenSource();
        }
        
        /// <summary>
        /// 当对象被激活时调用此方法。
        /// </summary>
        protected internal override void OnSpawn()
        {
            // 调用基类的OnSpawn方法
            base.OnSpawn();

            // 尝试将目标游戏对象设置为活跃状态
            (Target as GameObject)?.SetActive(true);
        }


        /// <summary>
        /// 当对象被销毁时执行的处理。
        /// </summary>
        protected internal override void OnDespawn()
        {
            // 调用基类的OnDespawn方法
            base.OnDespawn();

            // 将目标对象设置为非激活状态
            GameObject target = Target as GameObject;
            target.SetActive(false);

            // 如果存在AudioRoot，将目标对象的父对象设置为AudioRoot，保持其在场景中的位置不变
            if (AudioRoot)
                target.transform.SetParent(AudioRoot, false);
        }


        /// <summary>
        /// 释放音频对象。
        /// </summary>
        protected internal override void Release()
        {
            if (m_audioSource.isPlaying)
            {
                m_audioSource.Stop();
            }

            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
            
            if (Target != null)
            {
                GameObject.Destroy(Target as GameObject);
            }
            
            Target = null;
            m_audioSource = null;
        }
        
        
        /// <summary>
        /// 检查音频源是否正在播放音频。
        /// </summary>
        /// <returns>如果音频源正在播放，则返回true；否则返回false。</returns>
        public bool IsPlaying => m_audioSource.isPlaying;


        /// <summary>
        /// 播放音频。
        /// </summary>
        /// <param name="fadeInSeconds">声音淡入的时间，单位为秒。如果不需要淡入，则设置为0。</param>
        public void Play(float fadeInSeconds = 0)
        {
            // 取消之前的播放控制任务，以确保不会同时进行多个播放操作
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }

            // 开始播放音频
            m_audioSource.Play();
            if (fadeInSeconds > 0)
            {
                // 如果需要淡入，则先将音量设置为0，然后开始淡入操作
                m_audioSource.volume = 0;
                cts = new CancellationTokenSource();
                VolumeAsync(fadeInSeconds,true,null ,cts.Token).Forget();
            }
            else
            {
                // 如果不需要淡入，则直接将音量设置为最大
                m_audioSource.volume = 1;
            }
        }


        /// <summary>
        /// 停止声音播放，并通过渐变方式淡出。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出所需的时间，以秒为单位。</param>
        public void Stop(float fadeOutSeconds = 0)

        {
            if (!m_audioSource.isPlaying)
            {
                return;
            }
            cts.Cancel();
            cts.Dispose();
            if (fadeOutSeconds > 0)
            {
                cts = new CancellationTokenSource();
                VolumeAsync(fadeOutSeconds, false, () => { m_audioSource.Stop(); }, cts.Token).Forget();
            }
            else
            {
                m_audioSource.Stop();
            }
        }

        /// <summary>
        /// 暂停声音播放，并通过渐变方式淡出。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出所需的时间，以秒为单位。</param>
        public void Pause(float fadeOutSeconds = 0)
        {
            cts.Cancel();
            cts.Dispose();
            if (fadeOutSeconds > 0)
            {
                cts = new CancellationTokenSource();
                VolumeAsync(fadeOutSeconds, false, () => { m_audioSource.Pause(); }, cts.Token).Forget();
            }
            else
            {
                m_audioSource.Pause();
            }
        }

        /// <summary>
        /// 恢复声音播放，并通过渐变方式淡入。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡入所需的时间，以秒为单位。</param>
        public void Resume(float fadeOutSeconds = 0)
        {
            m_audioSource.Play();
            cts.Cancel();
            cts.Dispose();
            if (fadeOutSeconds > 0)
            {
                cts = new CancellationTokenSource();
                VolumeAsync(fadeOutSeconds, true, null, cts.Token).Forget();
            }
            else
            {
                m_audioSource.volume = 1;
            }
        }

        /// <summary>
        /// 设置音频源的输出混音器组。
        /// </summary>
        /// <param name="audioGroup">音频组，用于确定音频源的输出混音器组。</param>
        /// <exception cref="GameFrameworkException">当音频组为null时，抛出异常。</exception>
        public void SetOutput(AudioGroup audioGroup)
        {
            // 检查音频组是否有效，如果为null则抛出异常
            if (audioGroup == null)
            {
                throw new GameFrameworkException("AudioGroup is invalid.");
            }
            // 设置音频源的输出混音器组为音频组的指定混音器组
            m_audioSource.outputAudioMixerGroup = audioGroup.AudioGroupSetting.AudioMixerGroup;
        }


        

        public async UniTask VolumeAsync(float fadeSeconds,bool isIn,Action onComplete ,CancellationToken ctk)
        {
            float timer = 0;
            float fromValue = m_audioSource.volume,toValue = 1;
            if (!isIn)
            {
                toValue = 0;
            }
            while (timer < fadeSeconds)
            {
                if (ctk.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                timer += Time.deltaTime;
                m_audioSource.volume = Mathf.SmoothStep(fromValue, toValue, timer / fadeSeconds);
                await UniTask.Yield();
            }
            onComplete?.Invoke();
            
        }

        /// <summary>
        /// 音频源的静音属性。
        /// </summary>
        public bool Mute
        {
            get => m_audioSource.mute;
            set => m_audioSource.mute = value;
        }
        
        /// <summary>
        /// 音频源的循环属性。
        /// </summary>
        public bool Loop
        {
            get => m_audioSource.loop;
            set => m_audioSource.loop = value;
        }
        
        /// <summary>
        /// 音频源的优先级属性。
        /// </summary>
        public int Priority 
        {
            get => m_audioSource.priority;
            set => m_audioSource.priority = value;
        }
        
        /// <summary>
        /// 音频源的音量属性。
        /// </summary>
        public float Volume 
        {
            get => m_audioSource.volume;
            set => m_audioSource.volume = value;
        }
        
        /// <summary>
        /// 音频源的音高属性。
        /// </summary>
        public float Pitch 
        {
            get => m_audioSource.pitch;
            set => m_audioSource.pitch = value;
        }
        
        /// <summary>
        /// 音频源的立体声平衡属性。
        /// </summary>
        public float Stereo 
        {
            get => m_audioSource.panStereo;
            set => m_audioSource.panStereo = value;
        }
        
        /// <summary>
        /// 音频源的空间混合属性。
        /// </summary>
        public float SpatialBlend
        {
            get => m_audioSource.spatialBlend;
            set => m_audioSource.spatialBlend = value;
        }
    }
}