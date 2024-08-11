using AOT.Framework.Debug;
using AOT.Framework.Mvc;
using AOT.Framework.Network;
using HotUpdate.Controller;
using HotUpdate.Network.Message;

namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:INetworkDispatcher
    {
        public static void OnResLogin(ResLogin resLogin)
        {
            EDebug.Log($"OnResLogin: {resLogin.result}");
            MvcManager.Instance.GetController<LoginController>().OnResLogin(resLogin);
        }
    }
}