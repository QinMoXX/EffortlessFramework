using System;

namespace AOT.Framework.Audio
{
    public interface IAudioHandle
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Guid { get; }
        
        /// <summary>
        /// 获取目标对象实体
        /// </summary>
        public  object Target { get; }
        
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="fadeInSeconds">音淡入时间，以秒为单位。</param>
        public  void Play(float fadeInSeconds);
        
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="fadeOutSeconds">音淡出时间，以秒为单位。</param>
        public   void Stop(float fadeOutSeconds);
        
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="fadeOutSeconds">音淡出时间，以秒为单位。</param>
        public  void Pause(float fadeOutSeconds);
        
        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="fadeOutSeconds">音淡出时间，以秒为单位。</param>
        public  void Resume(float fadeOutSeconds);

        /// <summary>
        /// 设置输出
        /// </summary>
        /// <param name="audioGroup">音组</param>
        public void SetOutput(AudioGroup audioGroup);
        
        /// <summary>
        /// 是否播放
        /// </summary>
        public bool IsPlaying { get; }
        
        /// <summary>
        /// 静音
        /// </summary>
        public bool Mute { get; set; }

        /// <summary>
        /// 循环
        /// </summary>
        public  bool Loop { get; set; }
        
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// 音调
        /// </summary>
        public float Pitch { get; set; }
        
        /// <summary>
        /// 立体声
        /// </summary>
        public float Stereo { get; set; }

        /// <summary>
        /// 空间化计算
        /// </summary>
        public float SpatialBlend { get; set; }
    }
}