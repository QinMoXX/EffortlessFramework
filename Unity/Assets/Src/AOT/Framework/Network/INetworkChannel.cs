using System.Net;
using Cysharp.Threading.Tasks;

namespace AOT.Framework.Network
{
    public interface INetworkChannel
    {
        public IPEndPoint EndPoint { get; }
        
        public bool Connect(int port, IPEndPoint endPoint);

        public UniTaskVoid SendAsync(int id,byte[] data);

        public void Send(int id,byte[] data);

        public UniTask<byte[]> ReceiveAsync();

        public void Update(float virtualElapse, float realElapse);

        public void Close();
    }
}