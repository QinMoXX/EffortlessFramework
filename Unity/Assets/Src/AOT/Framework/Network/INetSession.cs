namespace AOT.Framework.Network
{
    /// <summary>
    /// 会话接口
    /// </summary>
    public interface INetSession
    {
        /// <summary>
        /// 会话Id
        /// </summary>
        public int SessionId { get; }
        
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected { get; }

        /// <summary>
        /// 心跳检测
        /// </summary>
        public void HeartbeatCheck();
    }
}