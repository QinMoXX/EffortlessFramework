using System;
using AOT.Framework.ObjectPool;

namespace AOT.Framework.Audio
{
    public interface IAudioManager
    {
        public IAudioManager Initialize(IObjectPoolManager resourceManager,object param);
        
        public Guid PlayAudio(string audioName, string groupName, float fadeInSeconds);
        
        public void StopAudio(Guid audioId, float fadeOutSeconds);

        public void ResumeAudio(Guid audioId, float fadeInSeconds);
        
        public void PauseAudio(Guid audioId, float fadeOutSeconds);

        public float GetAudioVolume(string groupName);
        
        public void SetAudioVolume(string groupName, float volume);

        public void InitAudioGroup();
        
        public AudioGroup GetAudioGroup(string groupName);
    }
}