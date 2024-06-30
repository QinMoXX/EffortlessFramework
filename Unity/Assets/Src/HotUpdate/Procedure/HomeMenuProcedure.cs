using System;
using System.Threading.Tasks;
using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Procedure;
using Cysharp.Threading.Tasks;
using HotUpdate.UI;
using HotUpdate.UI.View;
using HotUpdate.Utility;

public class HomeMenuProcedure:ProcedureBase
{
    public HomeMenuProcedure(IFsm handle) : base(handle)
    {
    }

    protected override void OnInit()
    {
        EDebug.Log("初始主菜单流程");
    }

    protected override async void OnEnter()
    {
        EDebug.Log("进入主菜单");

        OnUpdateAsync().Forget();
    }

    private async UniTaskVoid OnUpdateAsync()
    {
        await UIUtility.ShowAndTryCreateUIForm<LoginUI>(UIID.LoginUI);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        
    }

    protected override void OnLeave()
    {
        
    }
}