using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace AOT.Framework.Network
{
    public interface INetworkManager
    {
        public INetworkChannel CreateChannel<T>(int channelId,string address, int port) where T : INetworkChannel;
        
        public INetworkChannel GetChannel(int channelId);
        
        public void GetAllNetworkChannels(List<INetworkChannel> results);
    }
}