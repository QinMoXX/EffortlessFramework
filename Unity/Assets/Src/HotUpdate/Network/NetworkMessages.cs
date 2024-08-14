using System;
using AOT.Framework;
using MemoryPack;

namespace HotUpdate.Network.Message
{
    [MemoryPackable][PacketId(1000)]
    public partial class ReqLogin: NetPacket
    {
        public string name { get; set; }
        public string password { get; set; }
    }
    
    [MemoryPackable][PacketId(1001)]
    public partial class ResLogin: NetPacket
    {
        public bool result { get; set; }
        public string token { get; set; }
        
        public Int32 userId { get; set; }
    }
    
    [MemoryPackable][PacketId(1002)]
    public partial class ReqEntryRoom:NetPacket
    {
        public string token { get; set; }
    }
    
    [MemoryPackable][PacketId(1003)]
    public partial class ResEntryRoom:NetPacket
    {
        public Int32 roomId { get; set; }
        public bool result { get; set; }
    }
    
    
    [MemoryPackable][PacketId(-100)]
    public partial class FramSync: NetPacket
    {
        public Int32 FRAME_ID { get; set; }
        public Int32 BLOCK_ID { get; set; }
        
        public CMD CMD { get; set; }
        
        public CMD[] FRAME_SYNC { get; set; }
    }
    
    [MemoryPackable]
    public partial class CMD
    {
        public Int32 block_id { get; set; }
        public Int32 pid { get; set; }
        
        public Vector3Int pos { get; set; }
        
    }
    
    [MemoryPackable]
    public partial struct Vector3Int
    {
        public Int32 x{get;set;}
        public Int32 y{get;set;}
        public Int32 z{get;set;}
    }
}