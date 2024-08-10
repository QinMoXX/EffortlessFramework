using System.Collections.Generic;
using UnityEngine;

namespace AOT.Framework.Debug
{
    [CreateAssetMenu(fileName = "EDebugSetting", menuName = "Framework/EDebugSetting")]
    public class EDebugSetting:ScriptableObject
    {
        [Header("日志源")]
        public string[] LogSource;
    }
}