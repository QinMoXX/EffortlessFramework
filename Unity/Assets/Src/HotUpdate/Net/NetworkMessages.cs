using AOT.Framework;
using MemoryPack;

namespace HotUpdate.Network.Message
{
    [MemoryPackable][PacketId(1)]
    public partial class ReqLogin: NetPacket
    {
        public string name { get; set; }
        public string password { get; set; }
    }
    
    [MemoryPackable][PacketId(2)]
    public partial class ResLogin: NetPacket
    {
        public bool result { get; set; }
        
    }
}