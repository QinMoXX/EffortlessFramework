using System;

namespace AOT.Framework.Debug
{
    [Flags]
    public enum LogType: byte
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