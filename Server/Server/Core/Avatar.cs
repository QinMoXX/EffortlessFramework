using AOT.Framework.Network;
using Server.DBModel;

namespace Server.Core;

/// <summary>
/// 玩家类
/// </summary>
public class Avatar:IEquatable<Avatar>
{
    private NetSession m_session;
    private User m_user;
    public  Int32 UID { get =>m_user.UID;}
    

    public bool Equals(Avatar? other)
    {
        return UID == other.UID;
    }

    public override bool Equals(object? obj)
    {
        return Equals((Avatar)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(m_session, UID);
    }

    /// <summary>
    /// 创建Avatar
    /// </summary>
    /// <param name="session">会话</param>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    public static Avatar Create(NetSession session,User user)
    {
        var avatar = new Avatar();
        avatar.m_session = session;
        avatar.m_user = user;
        return  avatar;
    }
}