using System.Collections.Concurrent;
using AOT.Framework;
using AOT.Framework.Network;
using Server.Core;

namespace Server.Service;

public class HallsService:SingletonInstance<HallsService>,IService
{
    public ConcurrentDictionary<Int32,Avatar> m_halls_Avatar;
    
    public ConcurrentDictionary<Int32,Room> m_halls_Rooms;
    public Int32 lastNewRoomId; //最后一次新建房间Id
    
    private Guid guidCreater;
    public void Init()
    {
        m_halls_Avatar = new ConcurrentDictionary<Int32,Avatar>();
        m_halls_Rooms = new ConcurrentDictionary<Int32,Room>();
        lastNewRoomId = 0;
    }

    public void JoinAvatar(Avatar avatar)
    {
        if (m_halls_Avatar.TryGetValue(avatar.UID, out var _))
        {
            Console.WriteLine($"Player :{avatar.UID} Repeat to join the hall");
        }
        m_halls_Avatar.TryAdd(avatar.UID,avatar);
    }

    public void LeaveAvatar(Avatar avatar)
    {
        if (m_halls_Avatar.TryRemove(avatar.UID, out var _))
        {
            Console.WriteLine($"玩家 :{avatar.UID} exit the hall");
        }
    }

    public Room EntryRoom(Avatar avatar)
    {
        // 查找房间, 匹配最近开设的房间,如果房间满了, 则创建房间
        var room = FindRoom();
        // 进入一个玩家
        room.roomPlayerCount++;
        room.roomEntityCall.Add(avatar);
        // 从移除大厅
        m_halls_Avatar.TryRemove(avatar.UID,out var _);
        return room;
    }

    private Room FindRoom()
    {
        Room result = null;
        if (m_halls_Rooms.TryGetValue(this.lastNewRoomId, out result))
        {
            if (result.roomPlayerCount < GameSetting.MaxRoomPlayerCount)
            {
                return result;
            }
        }
        // 创建房间
        lastNewRoomId = Guid.NewGuid().GetHashCode();
        result = new Room(lastNewRoomId);
        m_halls_Rooms.TryAdd(lastNewRoomId, result);
        return result;
    }
    

    public void Start()
    {
        m_halls_Avatar.Clear();
    }

    public void Update()
    {
        
    }

    public void Destroy()
    {
        Console.WriteLine("Forced closure of lobby services");
    }
    
    public class Room
    {
        public Int32 roomId;
        public Int32 roomPlayerCount;
        public List<Avatar> roomEntityCall;
        public Room(Int32 roomId)
        {
            roomId = roomId;
            roomPlayerCount = 0;
            roomEntityCall = new List<Avatar>();
        }
    }
}