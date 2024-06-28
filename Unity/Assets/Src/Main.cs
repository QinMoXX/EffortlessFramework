using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework;
using HybridCLR;
using Src;
using Src.AOT.Framework.Debug;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using Src.AOT.Framework.Resource;
using UnityEngine;
using YooAsset;
using LogType = Src.AOT.Framework.Debug.LogType;

public class Main : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("资源加载模式")]
    public ResourceManager.ResourceRunningMode ResourceMode = ResourceManager.ResourceRunningMode.EDITOR;
#endif
    [Header("日志过滤")]
    public LogType LogFilter = LogType.Log | LogType.Warning | LogType.Warning;
    
    private void Awake()
    {
        EDebug.LogFilter = LogFilter;
        GameEntry.CreatModule<FsmManager>();
        GameEntry.CreatModule<ProcedureManager>();
        var resourceManager = GameEntry.CreatModule<ResourceManager>();
        //设置热更模式
#if UNITY_EDITOR
        resourceManager?.SetMode(ResourceMode);
#else
        resourceManager.SetMode(ResourceManager.ResourceRunningMode.ONLINE);
#endif
        
    }

    
    async UniTask Start()
    {
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(VersionCheckProcedure)
            ,typeof(DownloadProcedure)
            ,typeof(EnterGameProcedure));
        GameEntry.GetModule<ProcedureManager>().StartProcedure<VersionCheckProcedure>();
    }

    
    void Update()
    {
        GameEntry.Update(Time.deltaTime, Time.deltaTime);
    }
}
