using Cysharp.Threading.Tasks;

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

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="id">消息Id</param>
        /// <param name="messagePack">消息包</param>
        /// <typeparam name="T">消息包类型</typeparam>
        /// <returns></returns>
        public UniTask SendMessage<T>(int id, T messagePack);

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public string GetToken();
    }
}