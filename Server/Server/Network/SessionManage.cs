using System.Collections.Concurrent;
using System.Net;

namespace AOT.Framework.Network
{
    public class SessionManage:SingletonInstance<SessionManage>
    {
        private ConcurrentDictionary<int, NetSession> sessions = new ConcurrentDictionary<int, NetSession>();
        
        public void AddSession(int id,NetSession session)
        {
            if (sessions.TryGetValue(id, out var _))
            {
                return;
            }

            if (session == null)
            {
                throw  new ArgumentNullException($"Session is invalid");
            }

            sessions.TryAdd(id, session);
        }

        public NetSession? GetSession(int id)
        {
            if (sessions.TryGetValue(id, out var session))
            {
                return session;
            }
            return null;
        }

        public bool HasSession(int id)
        {
            return sessions.ContainsKey(id);
        }

        public static int GetSessionId(IPEndPoint endPoint)
        {
            //根据网络地址以及端口计算唯一int值
            return CalculateUniqueInt(endPoint);
        }
        
        /// <summary>
        /// 根据 IPEndPoint 计算一个唯一的整数值。
        /// 注意: 这个方法可能在不同的 IP 和端口组合下产生相同的整数值，
        /// 因此仅适用于 IP 地址和端口范围有限的情况。
        /// </summary>
        /// <param name="endPoint">IPEndPoint</param>
        /// <returns>计算出的唯一整数值</returns>
        private static int CalculateUniqueInt(IPEndPoint endPoint)
        {
            // 获取 IP 地址的字节表示
            byte[] addressBytes = endPoint.Address.GetAddressBytes();

            // 检查是否为 IPv4 地址
            if (addressBytes.Length != 4)
                throw new ArgumentException("The address must be an IPv4 address.");

            // 将 IP 地址的四个字节转换为整数
            int ipAddressAsInt = BitConverter.ToInt32(addressBytes, 0);

            // 将端口号转换为整数，并左移 32 位
            int portAsInt = (endPoint.Port << 32);

            // 将 IP 地址和端口号组合
            return portAsInt | ipAddressAsInt;
        }
    }

}

