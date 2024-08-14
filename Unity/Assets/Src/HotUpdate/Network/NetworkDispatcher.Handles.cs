using AOT.Framework.Debug;
using AOT.Framework.Mvc;
using AOT.Framework.Network;
using DG.DemiEditor;
using HotUpdate.Controller;
using HotUpdate.Network.Message;

namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:INetworkDispatcher
    {
        public static void OnResLogin(ResLogin resLogin)
        {
            EDebug.Log(Utility.String.Format("OnResLogin:{0}", resLogin.token));
            MvcManager.Instance.GetController<LoginController>().OnResLogin(resLogin);
        }

        public static void OnResEntryRoom(ResEntryRoom resEntryRoom)
        {
            EDebug.Log(Utility.String.Format("OnResEntryRoom:{0}", resEntryRoom.roomId));
            MvcManager.Instance.GetController<MenuController>().OnResEnterRoom(resEntryRoom);
        }
    }
}