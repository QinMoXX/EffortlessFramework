using System.Buffers;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using AOT.Framework.Network;
using Cysharp.Threading.Tasks;
using HotUpdate.Network.Message;

public class NetworkService: IService,IKcpCallback
{
    public SimpleSegManager.Kcp kcp { get; }
    
    private UdpClient m_client;

    private INetworkDispatcher? m_networkDispatcher; 
    
    public void Init()
    {
        
    }

    public void Start()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                kcp.Update(DateTimeOffset.UtcNow);
                NetSession[] sessions = SessionManage.Instance.GetAllSessions();
                for (int i = 0; i < sessions.Length; i++)
                {
                    sessions[i].Update(DateTimeOffset.UtcNow);
                    if (sessions[i].IsConnected == false)
                    {
                        SessionManage.Instance.RemoveSession(sessions[i].SessionId);
                    }
                }
                await Task.Delay(10);
            }
        });
        BeginRecv();
    }

    public void Update()
    {

    }

    public void Destroy()
    {
        
    }
    
    public NetworkService(int port, INetworkDispatcher networkDispatcher)
    {
        m_networkDispatcher = networkDispatcher;
        m_client = new UdpClient(port);
        kcp = new SimpleSegManager.Kcp(2001, this);
        kcp.TraceListener = new ConsoleTraceListener();
    }

    public void Output(IMemoryOwner<byte> buffer, int avalidLength)
    {
        var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
        m_client.SendAsync(s, s.Length, null);
        buffer.Dispose();
    }

    private async ValueTask<byte[]> ReceiveAsync(NetSession session)
    {
        var (buffer, avalidLength) = session.kcp.TryRecv();
        while (buffer == null)
        {
            await Task.Delay(10);
            (buffer, avalidLength) = session.kcp.TryRecv();
        }

        var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
        return s;
    }

    private async Task BeginRecv()
    {
        while (true)
        {
            var res = await m_client.ReceiveAsync();
            int sessionId = SessionManage.GetSessionId(res.RemoteEndPoint);
            NetSession session = SessionManage.Instance.GetSession(sessionId);
            if (session == null)
            {
                session = new NetSession(sessionId, res.RemoteEndPoint, this, m_client);
                SessionManage.Instance.AddSession(session.SessionId, session);
            }
            Console.WriteLine(($"Receive form session : {session.SessionId}, RemoteEndPoint:{session.EndPoint}"));
            session.Input(res.Buffer);

            ProcessMessage(session);
        }
    }
    
    private async Task ProcessMessage(NetSession session)
    {
        byte[] bin = await ReceiveAsync(session);
        var (id, data) = DetachPacket(bin);
        
        if (id < 0)
        {
            // id < 0 的为帧同步数据，不做处理，直接转发
            NetSession[] sessions = SessionManage.Instance.GetAllSessions();
            for (int i = 0; i < sessions.Length; i++)
            {
                if (session.SessionId == sessions[i].SessionId)
                {
                    continue;
                }
                sessions[i].SendAsync(bin);
            }
        }
        else
        {
            if (m_networkDispatcher == null)
            {
                throw  new Exception("Network Service is not initialized!");
            }
            m_networkDispatcher.Dispatch(session as INetSession, id , data);
        }
    }
    
    
    
    public static byte[] ConnectionPacket(int id,byte[] bin)
    {
        byte[] result = new byte[4 + bin.Length];
        
        result[0] = (byte)(id & 0xFF);          // 取最低字节
        result[1] = (byte)((id >> 8) & 0xFF);   // 取次低字节
        result[2] = (byte)((id >> 16) & 0xFF);  // 取次高字节
        result[3] = (byte)((id >> 24) & 0xFF);  // 取最高字节
        Buffer.BlockCopy(bin, 0, result, 4, bin.Length);
        return result;
    }

    public static (int, byte[]) DetachPacket(byte[] data)
    {
        int id = data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
        byte[] bin = new byte[data.Length - 4];
        Buffer.BlockCopy(data, 4, bin, 0, data.Length - 4);
        return (id, bin);
    }
}