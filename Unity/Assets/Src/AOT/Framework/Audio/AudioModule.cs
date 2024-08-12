using System;
using System.Collections.Generic;
using System.Linq;
using AOT.Framework.ObjectPool;
using AOT.Framework.Resource;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Profiling;

namespace AOT.Framework.Audio
{
    [DependencyModule(typeof(ResourceManager),typeof(ObjectPoolManager))]
    public class AudioManager:IGameModule,IAudioManager
    {
        private ObjectPoolManager.ObjectPool<AudioObject> spatialAudioObjectPool;
        private ObjectPoolManager.ObjectPool<AudioObject> planeAudioObjectPool;
        private Dictionary<Guid, AudioObject> m_AudioObjectIdMap;
        private IObjectPoolManager m_ObjectPoolManager;
        private Dictionary<string, AudioGroup> m_AudioGroupDict;
        public short Priority => 3;
        private AudioGroupSetting[] m_AudioGroupSettings;
        private Transform m_ObjectPoolRoot;
        public void Init()
        {
            m_AudioObjectIdMap = new Dictionary<Guid, AudioObject>();
            m_AudioGroupDict = new Dictionary<string, AudioGroup>();
        }

        public IAudioManager Initialize(IObjectPoolManager resourceManager,object param)
        {
            m_ObjectPoolManager = resourceManager;
            m_AudioGroupSettings = param as AudioGroupSetting[];
            spatialAudioObjectPool = m_ObjectPoolManager.CreateObjectPool<AudioObject>("spatialAudioObjectPool", false, 10, 40, 5, 10);
            planeAudioObjectPool = m_ObjectPoolManager.CreateObjectPool<AudioObject>("planeAudioObjectPool", true, 10, 100, 20, 20);
            InitAudioGroup();
            //创建音频对象池物理挂在对象
            m_ObjectPoolRoot = new GameObject("AudioObjectPoolRoot").transform;
            m_ObjectPoolRoot.SetParent(ObjectPoolManager.Root,false);
            AudioObject.AudioRoot = m_ObjectPoolRoot;
            return this;
        }

        public void Update(float virtualElapse, float realElapse)
        {
            Profiler.BeginSample("AudioManager.Update");
            Guid[] guids = m_AudioObjectIdMap.Keys.ToArray();
            for (int i = 0; i < guids.Length; i++)
            {
                if (!m_AudioObjectIdMap.TryGetValue(guids[i], out var audioObject))
                {
                    continue;
                }

                if (audioObject.IsPlaying)
                {
                    continue;
                }
                spatialAudioObjectPool.Despawn(audioObject.Target);
                planeAudioObjectPool.Despawn(audioObject.Target);
                m_AudioObjectIdMap.Remove(guids[i]);
            }
            Profiler.EndSample();
        }

        public void Destroy()
        {
            m_ObjectPoolManager.DestroyObjectPool<AudioObject>("spatialAudioObjectPool");
            spatialAudioObjectPool = null;
            m_ObjectPoolManager.DestroyObjectPool<AudioObject>("planeAudioObjectPool");
            spatialAudioObjectPool = null;
            m_ObjectPoolManager = null;
            m_AudioGroupDict.Clear();
            m_AudioGroupDict = null;
            m_ObjectPoolRoot = null;
            AudioObject.AudioRoot = null;
        }
        

        public IAudioHandle PlayAudio(string audioName, string groupName, float fadeInSeconds = 0)
        {
            if (string.IsNullOrEmpty(audioName))
            {
                throw new GameFrameworkException("Audio name is invalid");
            }

            if (string.IsNullOrEmpty(groupName))
            {
                throw new GameFrameworkException("Audio group name is invalid");
            }
            AudioGroup audioGroup = GetAudioGroup(groupName);
            ObjectPoolManager.Object<AudioObject> internalObj;
            if (audioGroup.AudioGroupSetting.AllowMultiSpawn)
            {
                internalObj = planeAudioObjectPool.Spawn(audioName) as ObjectPoolManager.Object<AudioObject>;
            }
            else
            {
                internalObj = spatialAudioObjectPool.Spawn(audioName) as ObjectPoolManager.Object<AudioObject>;
            }

            if (internalObj == null)
            {
                throw  new GameFrameworkException(Utility.String.Format("{0} Audio failed to play",audioName));
            }

            AudioObject audioObject = internalObj.GetObjectBase();
            audioObject.SetOutput(audioGroup);
            audioObject.Play(fadeInSeconds);
            Guid guid = Guid.NewGuid();
            audioObject.m_guid = guid;
            m_AudioObjectIdMap.Add(guid, audioObject);
            return audioObject;
        }

        public void StopAudio(Guid audioId, float fadeOutSeconds = 0)
        {
            if (!m_AudioObjectIdMap.TryGetValue(audioId, out var audioObject))
            {
                return;
            }
            audioObject.Stop(fadeOutSeconds);
        }

        public void ResumeAudio(Guid audioId, float fadeInSeconds = 0)
        {
            if (!m_AudioObjectIdMap.TryGetValue(audioId, out var audioObject))
            {
                return;
            }
            audioObject.Resume(fadeInSeconds);
        }

        public void PauseAudio(Guid audioId, float fadeOutSeconds = 0)
        {
            if (!m_AudioObjectIdMap.TryGetValue(audioId, out var audioObject))
            {
                return;
            }
            audioObject.Pause(fadeOutSeconds);
        }

        public float GetAudioVolume(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw  new GameFrameworkException("Audio group name is invalid");
            }

            if (!m_AudioGroupDict.TryGetValue(groupName, out var audioGroup))
            {
                throw  new GameFrameworkException(Utility.String.Format("{0} Audio group name is not exist",groupName));
            }

            return audioGroup.Volume;
        }

        public void SetAudioVolume(string groupName, float volume)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw  new GameFrameworkException("Audio group name is invalid");
            }

            if (!m_AudioGroupDict.TryGetValue(groupName, out var audioGroup))
            {
                throw  new GameFrameworkException(Utility.String.Format("{0} Audio group name is not exist",groupName));
            }
            audioGroup.Volume = volume;
        }

        /// <summary>
        /// 初始化音频组
        /// </summary>
        public void InitAudioGroup()
        {
            for (int i = 0; i < m_AudioGroupSettings.Length; i++)
            {
                var audioGroupSetting = m_AudioGroupSettings[i];
                if (audioGroupSetting == null | audioGroupSetting.AudioMixerGroup == null)
                {
                    throw new GameFrameworkException(Utility.String.Format("Audio group settings in the {0} index is invalid",i));
                }
                m_AudioGroupDict.Add(audioGroupSetting.AudioMixerGroup.name, new AudioGroup(audioGroupSetting.AudioMixerGroup.name, audioGroupSetting));
            }
        }

        /// <summary>
        /// 获取音频组
        /// </summary>
        /// <param name="groupName">音频组名</param>
        /// <returns></returns>
        /// <exception cref="GameFrameworkException"></exception>
        public AudioGroup GetAudioGroup(string groupName)
        {
            if (!m_AudioGroupDict.TryGetValue(groupName, out AudioGroup audioGroup))
            {
                throw new GameFrameworkException("Can not find audio group " + groupName);
            }

            return audioGroup;
        }
    }
}