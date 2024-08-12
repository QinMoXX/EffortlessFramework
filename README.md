# 简单的Unity开发框架
本框架尚处于初期开发阶段，功能尚待完善，仅作为个人探索与实践的成果。
旨在为 Unity 开发者提供一个轻量级、可扩展的游戏开发框架。该框架设计思想将逐步会以文章的形式登录个人博客[Blog](https://qinmo.world)。

框架汲取了 Hybridclr、Yooasset 和 Luan 的热更新技术，并融合了 GameFramework 的模块设计、状态机和流程控制思想，
力求在轻量级框架的基础上进行创新，探索更高效的游戏开发模式。为追求轻量又存在很多不一样的实现方式。

# 模块
- [x] 热更新（基于Yooasset）
- [x] 状态机
- [x] 流程
- [x] 数据表 （基于Luban）
- [x] 事件
- [x] MVC
- [x] UI
- [x] 音频
- [x] 对象池
- [x] 持久化数据
- [x] 场景
- [ ] 地图编辑器
- [ ] 实体
- [ ] 日志
- [ ] 网络

# 代码检查器
代码检查器负责编译和启动阶段的代码检查，包括代码格式、命名规范、代码规范、代码质量、代码效率等。

- 模块依赖检查：检测模块之间的依赖关系，确保模块之间没有循环依赖，并且确保模块加载时的顺序正确。
- 网络消息检查：检查消息定义合法性。包括消息ID重复检查

# UI
```csharp
// 打开界面
await UIUtility.ShowAndTryCreateUIForm<LoginUI>(UIID.LoginUI);

// 关闭界面
UIUtility.CloseUIForm(UIID.LoginUI);
```

# 音频
```csharp
// 播放音频
GameEntry.GetModule<AudioManager>().PlayAudio("Bullet Impact 21","UI");
// 获取音频Guid
IAudioHandle audioObject = GameEntry.GetModule<AudioManager>().PlayAudio("Railgun - Shot 6","Sound");
Guid audioId = audioObject.Guid;

// 停止音频
GameEntry.GetModule<AudioManager>().StopAudio(audioId,3); // 3秒淡出时间

// 空间音频
(audioObject.Target as GameObject).transform.position = Vector3.zero;
```

# 场景
```CSharp
//加载场景
GameEntry.GetModule<SceneManager>().LoadScene("Home", OnSceneLoadedActin) //同步
GameEntry.GetModule<SceneManager>().LoadSceneAsync("Home", default, OnSceneLoadedActin, OnSceneLoadingAction,false); //异步
private void OnSceneLoadingAction(float progress, string sceNename)
{
    EDebug.Log($"OnSceneLoadingAction progress: {progress.ToString()}");
}

private void OnSceneLoadedActin(bool isSuccess, string sceNename)
{
    //卸载场景
    GameEntry.GetModule<SceneManager>().UnloadScene("Main");
}
```

# 网络
本框架包含双端的网络框架，支持KCP协议，消息使用 MemoryPack 进行序列化。提供 .Net 平台上最快的struct序列化能力。


# 参考
本框架的开发离不开众多前辈的智慧结晶，包括 GameFramework、ET 和 QFramework 等，以及 Hybridclr、Luban、YooAsset 和 UniTask 等优秀工具和解决方案的支持。
我们将持续学习和借鉴，不断提升框架的完善度和实用性。

[GitHub - EllanJiang/GameFramework](https://github.com/EllanJiang/GameFramework)
[GitHub - egametang/ET](https://github.com/egametang/ET)
[GitHub - liangxiegame/QFramework](https://github.com/liangxiegame/QFramework)

[GitHub - focus-creative-games/hybridclr](https://github.com/focus-creative-games/hybridclr)
[GitHub - focus-creative-games/luban](https://github.com/focus-creative-games/luban)
[GitHub - tuyoogame/YooAsset](https://github.com/tuyoogame/YooAsset)
[GitHub - Cysharp/UniTask](https://github.com/Cysharp/UniTask)
