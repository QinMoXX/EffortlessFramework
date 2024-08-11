using AOT.Framework.Network;
using HotUpdate.Network.Message;

namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:INetworkDispatcher
    {
        public static void OnReqLogin(INetSession session, ReqLogin resLogin)
        {
            Console.WriteLine($"OnReqLogin: {resLogin.name} {resLogin.password}");
            session.SendMessage<ResLogin>((int)NetworkMessageIds.ResLogin,
                new ResLogin {  result = true, token = session.SessionId.ToString()});
        }
    }
}