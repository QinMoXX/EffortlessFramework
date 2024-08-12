using AOT.Framework.Network;
using HotUpdate.Network.Message;
using MongoDB.Driver;
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
                        result = true, token = session.SessionId.ToString()
                    });
                    var update = Builders<User>.Update.Set(user => user.LastLoginTime, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    DBService.Instance.UpdateDataAsync<User>("User", filter, update);

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
    }
}