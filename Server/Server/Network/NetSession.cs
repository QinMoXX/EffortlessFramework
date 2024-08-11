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
        const int HEARTBEAT_INTERVAL = 1000 * 10; //单位毫秒
        private int m_sessionId;
        private IPEndPoint m_endPoint;
        private NetworkService m_service;
        private bool m_isConnected;
        
        public SimpleSegManager.Kcp kcp { get; }
        private UdpClient m_client;

        private long m_lastLiftTime; //单位毫秒
        
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
            this.kcp.TraceListener = new ConsoleTraceListener();
            m_lastLiftTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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

        public async UniTask SendAsync(byte[] datagram)
        {
            kcp.Send(datagram.AsSpan().Slice(0, datagram.Length));
        }
        
        public async ValueTask<byte[]> ReceiveAsync()
        {
            var (buffer, avalidLength) = kcp.TryRecv();
            // 等待计数器
            int count = 0;
            while (buffer == null)
            {
                await Task.Delay(10);
                (buffer, avalidLength) = kcp.TryRecv();
                count += 1;
                if (count > 10)
                {
                    return Array.Empty<byte>();
                }
            }

            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            return s;
        }

        public void Input(byte[] buffer)
        {
            kcp.Input(buffer);
            HeartbeatCheck();
        }
        
        public void HeartbeatCheck()
        {
            Interlocked.Exchange(ref m_lastLiftTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
        
        public void Update(in DateTimeOffset time)
        {
            kcp.Update(time);
            if (time.ToUnixTimeMilliseconds() - Interlocked.Read(ref m_lastLiftTime) > HEARTBEAT_INTERVAL)
            {
                // m_isConnected = false;
            }
        }

        public void Dispose()
        {
            m_client = null;
            m_endPoint = null;
            m_service = null;
            m_isConnected = false;
            kcp?.Dispose();
        }

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            m_client.SendAsync(s, s.Length, m_endPoint);
            buffer.Dispose();
        }
    }
}
