using AOT.Framework.Network;
using HotUpdate.Network.Message;
using MongoDB.Driver;
using Server.Core;
using Server.DBModel;
using Server.Service;

namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:INetworkDispatcher
    {
        public static void OnReqLogin(INetSession session, ReqLogin reqLogin)
        {
            Console.WriteLine($"OnReqLogin: {reqLogin.name} {reqLogin.password}");
            try
            {
                var filter = Builders<User>.Filter
                    .Eq(user => user.UserName, reqLogin.name);
                User result = DBService.Instance.FindData<User>("User", filter);
                if (result != null && result.Password == reqLogin.password)
                {
                    session.SendMessage<ResLogin>((int)NetworkMessageIds.ResLogin, new ResLogin
                    {
                        result = true, token = session.GetToken(),userId = result.UID
                    });
                    var update = Builders<User>.Update.Set(user => user.LastLoginTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    DBService.Instance.UpdateDataAsync<User>("User", filter, update);
                    //登录成功
                    Avatar avatar = Avatar.Create(session as NetSession, result);
                    SessionManage.Instance.AddAvatar(session.SessionId, avatar);
                    HallsService.Instance.JoinAvatar(avatar);
                }
                else
                {
                    session.SendMessage<ResLogin>((int)NetworkMessageIds.ResLogin, new ResLogin()
                    {
                        result = false
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("OnReqLogin Failed! "+ e);
            }

        }

        public static void OnReqEntryRoom(INetSession session, ReqEntryRoom reqEntryRoom)
        {
            if (reqEntryRoom == null)
            {
                return;
            }
            Console.WriteLine($"OnReqEntryRoom: {reqEntryRoom.token}");
            if (session.GetToken() != reqEntryRoom.token)
            {
                Console.WriteLine("OnReqEntryRoom Failed! Token is invalid!");
            }

            Avatar avatar = SessionManage.Instance.GetAvatar(session.SessionId);

            if (avatar == null)
            {
                Console.WriteLine("OnReqEntryRoom Failed! Avatar is not exist in Halls!");
                return;
            }

            //离开大厅
            HallsService.Instance.LeaveAvatar(avatar);
            try
            {
                var room = HallsService.Instance.EntryRoom(avatar);
                session.SendMessage<ResEntryRoom>((int)NetworkMessageIds.ResEntryRoom, new ResEntryRoom()
                {
                    roomId = room.roomId,
                    result = true,
                });
            }
            catch (Exception e)
            {
                // 如果出现进入房间失败，则返回大厅
                HallsService.Instance.JoinAvatar(avatar);
            }

        }
    }
}