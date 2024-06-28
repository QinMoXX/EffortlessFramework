namespace Src.AOT.Framework.Debug
{
    /// <summary>
    /// 日志辅助接口
    /// </summary>
    public interface ILogHelper
    {
        /// <summary>
        /// 日子载入
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">日志</param>
        public void LogInfo(LogType type, string message);
    }
}