using AOT.Framework.Mvc;
using HotUpdate.Network.Message;

namespace HotUpdate.Controller
{
    public sealed class MenuController : IController
    {
        public void Init()
        {

        }

        public void OnResEnterRoom(ResEntryRoom resEntryRoom)
        {
            if (resEntryRoom.result)
            {
                
            }
        }
    }
}