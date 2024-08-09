using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
using MemoryPack;

namespace AOT.Framework.Network
{
    public class NetSession:IDisposable,INetSession,IKcpCallback
    {
        const int conv = 2001;
        private int m_sessionId;
        private IPEndPoint m_endPoint;
        private NetworkService m_service;
        private bool m_isConnected;
        
        public SimpleSegManager.Kcp kcp { get; }
        private UdpClient m_client;

        public int SessionId
        {
            get => m_sessionId;
        }

        public bool IsConnected
        {
            get
            {
                return m_isConnected && m_service != null;
            }
        }

        public IPEndPoint EndPoint
        {
            get => m_endPoint;
        }
        
        public NetSession(int sessionId, IPEndPoint endPoint, NetworkService service, UdpClient client)
        {
            this.m_sessionId = sessionId;
            this.m_endPoint = endPoint;
            this.m_service = service;
            m_isConnected = true;
            this.m_client = client;
            
            this.kcp = new SimpleSegManager.Kcp(conv, this);
        }
        

        public async UniTask SendMessage<T>(int id, T messagePack)
        {
            if (messagePack == null)
            {
                Console.WriteLine("Send messagePack is null");
                return;
            }

            if (!IsConnected)
            {
                Console.WriteLine($"Session :{m_sessionId} is disconnected");
            }
            var datagram = NetworkService.ConnectionPacket(id, MemoryPackSerializer.Serialize(messagePack));
            kcp.Send(datagram.AsSpan().Slice(0, datagram.Length));
        }

        public void HeartbeatCheck()
        {
            
        }
        
        public void Update(in DateTimeOffset time)
        {
            kcp.Update(time);
        }

        public void Dispose()
        {
            m_endPoint = null;
            m_service = null;
            m_isConnected = false;
        }

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            m_client.SendAsync(s, s.Length, m_endPoint);
            buffer.Dispose();
        }
    }
}
