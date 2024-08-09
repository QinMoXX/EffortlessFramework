using System.Buffers;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using AOT.Framework.Network;
using GameServer.Core;
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
        Task.Run(BeginRecv);
        Task.Run(async () =>
        {
            while (true)
            {
                kcp.Update(DateTimeOffset.UtcNow);
                await Task.Delay(10);
            }
        });
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
        
    }

    public void Output(IMemoryOwner<byte> buffer, int avalidLength)
    {
        var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
        m_client.SendAsync(s, s.Length, null);
        buffer.Dispose();
    }

    private async void SendAsync<T>(NetSession session, int id,T messagePack) where  T : NetPacket
    {
        await session.SendAsync<T>(id, messagePack);
    }

    private async ValueTask<byte[]> ReceiveAsync(NetSession session)
    {
        var (buffer, avalidLength) = kcp.TryRecv();
        while (buffer == null)
        {
            await Task.Delay(10);
            (buffer, avalidLength) = kcp.TryRecv();
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
            NetSession? session = SessionManage.Instance.GetSession(sessionId);
            if (session == null)
            {
                session = new NetSession(sessionId, res.RemoteEndPoint, this, m_client, kcp);
                SessionManage.Instance.AddSession(session.SessionId, session);
            }
            kcp.Input(res.Buffer);
            
            ProcessMessage(session);
        }
    }
    
    private async Task ProcessMessage(NetSession session)
    {
        byte[] bin = await ReceiveAsync(session);
        var (id, data) = DetachPacket(bin);
        if (m_networkDispatcher == null)
        {
            throw  new Exception("Network Service is not initialized!");
        }
        m_networkDispatcher.Dispatch(session as INetSession, id , data);
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