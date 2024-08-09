using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace AOT.Framework.Network
{
    public interface INetworkManager
    {
        public INetworkChannel AddChannel(int channelId, INetworkChannel channel);
        
        public INetworkChannel GetChannel(int channelId);
        
        public IList<INetworkChannel> GetAllNetworkChannels();
    }
}