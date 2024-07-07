using System.Collections.Generic;
using AOT.Framework.ObjectPool;
using AOT.Framework.Resource;
using UnityEngine.Audio;

namespace AOT.Framework.Audio
{
    public class AudioGroup
    {
        private AudioGroupSetting m_AudioGroupSetting;
        private string m_GroupName;
        public  string GroupName { get => m_GroupName; }
        
        public AudioGroup(string groupName,AudioGroupSetting audioGroupSetting)
        {
            this.m_GroupName = groupName;
            this.m_AudioGroupSetting = audioGroupSetting;
        }
        
        /// <summary>
        /// 获取AudioGroupSetting
        /// </summary>
        public AudioGroupSetting AudioGroupSetting
        {
            get
            {
                return m_AudioGroupSetting;
            }
        }
        
        /// <summary>
        /// 获取AudioGroup的音量
        /// </summary>
        public float Volume
        {
            get
            {
                if (AudioGroupSetting.AudioMixerGroup.audioMixer.GetFloat(m_GroupName, out float value))
                {
                    return value;
                }
                return 0;
            }
            set
            {
                AudioGroupSetting.AudioMixerGroup.audioMixer.SetFloat(AudioGroupSetting.GroupName,value);
            }
        }
        
    }
}