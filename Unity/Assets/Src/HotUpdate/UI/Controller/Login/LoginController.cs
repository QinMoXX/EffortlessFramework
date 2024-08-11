using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Mvc;
using AOT.Framework.Network;
using Cysharp.Threading.Tasks;
using Framework.UI;
using HotUpdate.Model;
using HotUpdate.Network.Message;
using HotUpdate.UI.View;
using HotUpdate.Utility;
using MemoryPack;

namespace HotUpdate.Controller
{
    public class LoginController:IController
    {
        public string name = "LoginController";
        public void Init()
        {
            
        }

        public void Login(string name, string password)
        {
            ReqLogin reqLogin = new ReqLogin() { name = name, password = password };
            this.GetModel<LoginModel>().userName = name;
            KcpChannel channel = GameEntry.GetModule<NetworkManager>().GetChannel(1) as KcpChannel;
            channel.SendAsync(1000, MemoryPackSerializer.Serialize(reqLogin));
        }

        public void OnResLogin(ResLogin resLogin)
        {
            if (resLogin.result)
            {
                //登录成功
                UIUtility.CloseUIForm(UIID.LoginUI);
                this.GetModel<LoginModel>().token = resLogin.token;
                EDebug.Log($"登录成功！{this.GetModel<LoginModel>().userName} token : {this.GetModel<LoginModel>().token}");
            }
            else
            {
                //登录失败
                EDebug.Log("登录失败！");
            }
        }
    }
}