using System;
using UnityEngine;

namespace Src.AOT.Framework.Debug
{
    public interface ILogHelper
    {
        public void LogInfo(DebugType type, string message);
    }
        
    public static class EDebug
    {
        public static DebugType DebugFilter;

        public static void Log(string message)
        {
            if (DebugType.Log == DebugFilter)
            {
                    
            }
                
        }
    }
        
        
    [Flags]
    public enum DebugType: byte
    {
        /// <summary>
        /// 信息。
        /// </summary>
        Log = 1,

        /// <summary>
        /// 警告。
        /// </summary>
        Warning = 2,

        /// <summary>
        /// 错误。
        /// </summary>
        Error = 4,

    }
}