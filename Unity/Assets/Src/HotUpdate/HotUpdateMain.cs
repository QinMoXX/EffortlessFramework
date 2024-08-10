using System;
using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Network;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using AOT.UI;
using Framework.UI;
using HotUpdate;
using HotUpdate.Network;
using HotUpdate.Network.Message;
using HotUpdate.UI.View;
using MemoryPack;

public class HotUpdateMain:IEntry
{
    public void Entry()
    {
        EDebug.Log("热更新代码加载成功");
        GameEntry.CreatModule<TableManager>().Initialize(GameEntry.GetModule<ResourceManager>());
        
        InitializeUI();
        
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(HomeMenuProcedure)).StartProcedure<HomeMenuProcedure>();
        
        // 测试网络
        NetworkDispatcher networkDispatcher = new NetworkDispatcher();
        KcpChannel channel = GameEntry.GetModule<NetworkManager>().GetChannel(1) as KcpChannel;
        channel.Initialize(networkDispatcher);
        
        networkDispatcher.SubscribeMessage<ResLogin>((int)NetworkMessageIds.ResLogin, NetworkDispatcher.OnResLogin);

        ReqLogin reqLogin = new ReqLogin() { name = "test", password = "123456" };
        channel.SendAsync(1000, MemoryPackSerializer.Serialize(reqLogin));
    }
    
    private void InitializeUI()
    {
        UIManager uiManager = GameEntry.GetModule<UIManager>();
        //初始化UI组
        Type uiGroupIdEnumType = typeof(UIGroupID);
        for (int i = 1; i <= Enum.GetValues(uiGroupIdEnumType).Length; i++)
        {
            uiManager.AddUIGroup(Enum.GetName(uiGroupIdEnumType, i), i);
        }
    }

    public void Exit()
    {
        EDebug.Log("退出热更新代码");
    }
}
