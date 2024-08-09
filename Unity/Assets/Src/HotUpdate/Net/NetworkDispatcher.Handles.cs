using AOT.Framework.Debug;
using AOT.Framework.Network;
using HotUpdate.Network.Message;

namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:INetworkDispatcher
    {
        public static void OnResLogin(ResLogin resLogin)
        {
            EDebug.Log($"OnResLogin: {resLogin.result}");
        }
    }
}