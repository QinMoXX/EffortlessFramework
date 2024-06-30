using System;
using AOT.Framework;
using Cysharp.Threading.Tasks;
using Src;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Mvc;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using AOT.UI;
using Framework.UI;
using HotUpdate.UI.View;
using UnityEngine;
using LogType = AOT.Framework.Debug.LogType;

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
        GameEntry.CreatModule<MvcManager>();
        GameEntry.CreatModule<UIManager>();
    }

    
    void Start()
    {
        InitializeUI();
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(VersionCheckProcedure)
            ,typeof(DownloadProcedure)
            ,typeof(EnterGameProcedure));
        GameEntry.GetModule<ProcedureManager>().StartProcedure<VersionCheckProcedure>();
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitializeUI()
    {
        UIManager uiManager = GameEntry.GetModule<UIManager>();
        uiManager.Initialize(new UIGroupHelper(),new UIFormHelper(uiManager,GameEntry.GetModule<ResourceManager>()));
        //初始化UI组
        Type uiGroupIdEnumType = typeof(UIGroupID);
        for (int i = 1; i <= Enum.GetValues(uiGroupIdEnumType).Length; i++)
        {
            uiManager.AddUIGroup(Enum.GetName(uiGroupIdEnumType, i), i);
        }
    }

    
    void Update()
    {
        GameEntry.Update(Time.deltaTime, Time.deltaTime);
    }
}
