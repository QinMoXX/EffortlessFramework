using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using System.Threading.Tasks;
using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Network;
using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;

public class KcpChannel : IKcpCallback,INetworkChannel
{
    UdpClient client;

    public SimpleSegManager.Kcp Kcp { get;private set; }
    public IPEndPoint EndPoint { get;private set; }
    
    private ConcurrentQueue<ValueTuple<int, byte[]>> m_messageQueue = new ConcurrentQueue<(int, byte[])>();

    private INetworkDispatcher m_networkDispatcher;
    
    public void Initialize(INetworkDispatcher networkDispatcher)
    {
        m_networkDispatcher = networkDispatcher;
    }

    public KcpChannel(int port, IPEndPoint endPoint)
    {
        Connect(port, endPoint);
    }

    public bool Connect(int port, IPEndPoint endPoint)
    {
        try
        {
            client = new UdpClient(port);
            Kcp = new SimpleSegManager.Kcp(2001, this);
            EndPoint = endPoint;
            UniTask.RunOnThreadPool(BeginRecv);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public void Update(float virtualElapse, float realElapse)
    {
        if (m_networkDispatcher == null || m_messageQueue == null)
        {
            return;
        }
        
        if (m_messageQueue.TryDequeue(out var v))
        {
            m_networkDispatcher.Dispatch(v.Item1, v.Item2);
        }
    }
    
    public async UniTaskVoid SendAsync(int id ,byte[] data)
    {
        if (data == null)
        {
            throw new GameFrameworkException("messagePacket is invalid.");
        }

        data = ConnectionPacket(id, data);
        Kcp.Send(data.AsSpan().Slice(0, data.Length));
    }
    

    public void Send(int id ,byte[] data)
    {
        if (data == null)
        {
            throw new GameFrameworkException("messagePacket is invalid.");
        }
        data = ConnectionPacket(id, data);
        Kcp.Send(data.AsSpan().Slice(0, data.Length));
    }

    public async UniTask<byte[]> ReceiveAsync()
    {
        var (buffer, avalidLength) = Kcp.TryRecv();
        while (buffer == null)
        {
            await Task.Delay(10);
            (buffer, avalidLength) = Kcp.TryRecv();
        }

        return buffer.Memory.Span.Slice(0, avalidLength).ToArray();
    }

    public void Close()
    {
        EndPoint = null;
        Kcp.Dispose();
        Kcp = null;
    }

    private async UniTaskVoid BeginRecv()
    {
        while (true)
        {
            var res = await client.ReceiveAsync();
            EndPoint = res.RemoteEndPoint;
            Kcp.Input(res.Buffer);
            byte[] bytes = await ReceiveAsync();
            m_messageQueue.Enqueue(DetachPacket(bytes));
        }
    }
    
    public void Output(IMemoryOwner<byte> buffer, int avalidLength)
    {
        var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
        client.SendAsync(s, s.Length, EndPoint);
        buffer.Dispose();
    }

    public byte[] ConnectionPacket(int id,byte[] bin)
    {
        byte[] result = new byte[4 + bin.Length];
        
        result[0] = (byte)(id & 0xFF);          // 取最低字节
        result[1] = (byte)((id >> 8) & 0xFF);   // 取次低字节
        result[2] = (byte)((id >> 16) & 0xFF);  // 取次高字节
        result[3] = (byte)((id >> 24) & 0xFF);  // 取最高字节
        EDebug.Log("result[4]: {result[4].ToString() }");
        Buffer.BlockCopy(bin, 0, result, 4, bin.Length);
        return result;
    }

    public ValueTuple<int, byte[]> DetachPacket(byte[] data)
    {
        int id = data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
        byte[] bin = new byte[data.Length - 4];
        Buffer.BlockCopy(data, 4, bin, 0, data.Length - 4);
        return (id, bin);
    }
}