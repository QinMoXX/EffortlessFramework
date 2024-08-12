using AOT.Environment;
using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Mvc;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using AOT.Framework.Scene;
using Cysharp.Threading.Tasks;
using HotUpdate.UI;
using HotUpdate.UI.View;
using HotUpdate.Utility;
using UnityEngine;

public class HomeMenuProcedure:ProcedureBase
{
    private bool enterMap;
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

        OnUpdateAsync().Forget();
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
        
    }

    protected override void OnLeave()
    {
        
    }
}