using System.Collections.Generic;
using UnityEngine;

namespace AOT.Framework.Debug
{
        
    public sealed class EDebug
    {
        public static ILogHelper LogHelper;
        public static LogType LogFilter;

        public static Dictionary<int, EDebug> m_DebugDict = new Dictionary<int, EDebug>();
        public static EDebug GetSource()
        {
            return new EDebug();
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            LogHelper?.LogInfo(LogType.Log, message);
            if ((LogType.Log & LogFilter) == 0)
            {
                return;
            }
            UnityEngine.Debug.Log(message);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            LogHelper?.LogInfo(LogType.Warning, message);
            if ((LogType.Warning & LogFilter) == 0)
            {
                return;
            }
            UnityEngine.Debug.LogWarning(message);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            LogHelper?.LogInfo(LogType.Error, message);
            if ((LogType.Error & LogFilter) == 0)
            {
                return;
            }
            UnityEngine.Debug.LogError(message);
        }
    }
        
        

}