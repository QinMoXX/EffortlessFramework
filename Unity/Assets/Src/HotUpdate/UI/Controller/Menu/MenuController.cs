using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Mvc;
using AOT.Framework.Network;
using HotUpdate.Model;
using HotUpdate.Network.Message;
using MemoryPack;

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
                EDebug.Log($"进入房间成功： roomId {resEntryRoom.roomId.ToString()}");
            }
        }

        public void ReqEntryRoom()
        {
            ReqEntryRoom reqEntryRoom = new ReqEntryRoom(){token = this.GetModel<LoginModel>().token};
            KcpChannel channel = GameEntry.GetModule<NetworkManager>().GetChannel(1) as KcpChannel;
            channel.SendAsync((int)NetworkMessageIds.ReqEntryRoom, MemoryPackSerializer.Serialize(reqEntryRoom));
        }
    }
}