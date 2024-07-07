using UnityEngine;
using UnityEngine.Audio;

namespace AOT.Framework.Audio
{
    [CreateAssetMenu(fileName = "AudioGroupSetting", menuName = "Framework/AudioGroupSetting")]
    public class AudioGroupSetting:ScriptableObject
    {
        public string GroupName;
        public AudioMixerGroup AudioMixerGroup;
        public bool AllowMultiSpawn;
    }
    
}