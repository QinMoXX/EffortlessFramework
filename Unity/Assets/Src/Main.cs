using System;
using AOT.Framework;
using AOT.Framework.Audio;
using Src;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Mvc;
using AOT.Framework.Network;
using AOT.Framework.ObjectPool;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using AOT.UI;
using Framework.UI;
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
    [Header("音频组设置")]
    public AudioGroupSetting[] audioGroupSettings = null;
    [Header("Host")]
    public int EndPort = 40001;

    public int Port = 5001;
    
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
        GameEntry.CreatModule<ObjectPoolManager>();
        GameEntry.CreatModule<AudioManager>();
        GameEntry.CreatModule<NetworkManager>().Initialize();
    }

    
    void Start()
    {
        UIManager uiManager = GameEntry.GetModule<UIManager>();
        uiManager.Initialize(new UIGroupHelper(),new UIFormHelper(uiManager,GameEntry.GetModule<ResourceManager>()));
        //初始化音频模块
        GameEntry.GetModule<AudioManager>().Initialize(GameEntry.GetModule<ObjectPoolManager>(),audioGroupSettings);
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(VersionCheckProcedure)
            ,typeof(DownloadProcedure)
            ,typeof(EnterGameProcedure))
            .StartProcedure<VersionCheckProcedure>();
        
        // 测试网络
        var endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, EndPort);
        KcpChannel channel = new KcpChannel(Port, endPoint);
        GameEntry.GetModule<NetworkManager>().AddChannel(1, channel);
    }
    

    
    void Update()
    {
        GameEntry.Update(Time.deltaTime, Time.deltaTime);
    }
}
