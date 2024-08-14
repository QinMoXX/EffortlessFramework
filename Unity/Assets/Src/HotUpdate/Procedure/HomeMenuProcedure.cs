using AOT.Environment;
using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Mvc;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using AOT.Framework.Scene;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotUpdate.Event;
using HotUpdate.UI;
using HotUpdate.UI.View;
using UnityEngine;

public class HomeMenuProcedure:ProcedureBase,IEventReceiver
{
    private bool EnterHome;
    public HomeMenuProcedure(IFsm handle) : base(handle)
    {
    }

    protected override void OnInit()
    {
        EDebug.Log("初始主菜单流程");
    }

    protected override void OnEnter()
    {
        EDebug.Log("进入主菜单界面");
        this.Register<EnterHomeEvent, WorldEventGroup>(OnEnterHomeEvent);
        OnUpdateAsync().Forget();
    }

    private void OnEnterHomeEvent(object sender, EnterHomeEvent e)
    {
        EnterHome = true;
        EnvCamera.MainCamera.transform.DOMoveZ(0, 2f).SetEase(Ease.InBack).SetDelay(0.5f);
        EDebug.Log("进入主界面");
    }

    private async UniTaskVoid OnUpdateAsync()
    {
        EnvCamera.MainCamera.transform.position = new Vector3(0, 0, -5);
        EnvCamera.MainCamera.gameObject.AddComponent<Skybox>().material = 
            await GameEntry.GetModule<ResourceManager>().LoadAssetAsync<Material>("Material_FS013_Day");
        GameEntry.GetModule<SceneManager>().LoadSceneAsync("Home", default, OnSceneLoadedActin, OnSceneLoadingAction,false);
        await UIUtility.ShowAndTryCreateUIForm<LoginUI>(UIID.LoginUI);
    }
    
    private void OnSceneLoadingAction(float progress, string sceNename)
    {
        EDebug.Log($"OnSceneLoadingAction progress: {progress.ToString()}");
    }

    private void OnSceneLoadedActin(bool isSuccess, string sceNename)
    {
        GameEntry.GetModule<SceneManager>().UnloadScene("Main");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (EnterHome)
        {
            
        }
    }

    protected override void OnLeave()
    {
        this.UnRegister<EnterHomeEvent,WorldEventGroup>(OnEnterHomeEvent);
    }
}