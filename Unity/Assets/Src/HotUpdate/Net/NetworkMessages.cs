using AOT.Framework.Network;
using MemoryPack;

namespace HotUpdate.Network.Message
{
    [MemoryPackable]
    public partial class ReqLogin: NetPacket
    {
        public string name { get; set; }
        public string password { get; set; }
    }
    
    [MemoryPackable]
    public partial class ResLogin: NetPacket
    {
        public bool result { get; set; }
        
    }
}