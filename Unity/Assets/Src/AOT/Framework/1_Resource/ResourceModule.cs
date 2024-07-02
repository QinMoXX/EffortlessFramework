using Cysharp.Threading.Tasks;
using YooAsset;


namespace AOT.Framework.Resource
{
    public partial class ResourceManager:IGameModule
    {
        private ResourceRunningMode m_mode = ResourceRunningMode.ONLINE;
        public short Priority => 1;

        public ResourcePackage defaultPackage;
        public string packageVersion;
        
        public bool IsInitialize { get;private set; }

        public void SetMode(ResourceRunningMode mode)
        {
            m_mode = mode;
        }
        
        public void Update(float virtualElapse, float realElapse)
        {
            
        }

        public void Destroy()
        {
            YooAssets.Destroy();
        }

        public void Init()
        {
            packageVersion = string.Empty;
        }

        public async UniTask Initialize()
        {
            // 初始化资源系统
            YooAssets.Initialize();
            // 创建默认的资源包
            defaultPackage = YooAssets.CreatePackage("DefaultPackage");
            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(defaultPackage);
            if (m_mode == ResourceRunningMode.STANDALONE)
            {
                //单机模式运行
                var initParameters = new OfflinePlayModeParameters();
                await defaultPackage.InitializeAsync(initParameters);
            }
            else if (m_mode == ResourceRunningMode.ONLINE)
            {
                await InitializeYooAsset(defaultPackage);
            }
#if UNITY_EDITOR
            else if(m_mode == ResourceRunningMode.EDITOR)
            {
                //编辑器模式运行
                var initParameters = new EditorSimulateModeParameters();
                var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
                initParameters.SimulateManifestFilePath  = simulateManifestFilePath;
                await defaultPackage.InitializeAsync(initParameters);
            }
#endif
            IsInitialize = true;
        }
        
        
        private UniTask InitializeYooAsset(ResourcePackage package)
        {
            // 注意：GameQueryServices.cs 太空战机的脚本类，详细见StreamingAssetsHelper.cs
            // string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";
            // string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";
            // var initParameters = new HostPlayModeParameters();
            // initParameters.BuildinQueryServices = new GameQueryServices(); 
            // initParameters.DecryptionServices = new FileOffsetDecryption();
            // initParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            // var initOperation = package.InitializeAsync(initParameters);
            // return initOperation;
            //
            // if(initOperation.Status == EOperationStatus.Succeed)
            // {
            //     Debug.Log("资源包初始化成功！");
            // }
            // else 
            // {
            //     Debug.LogError($"资源包初始化失败：{initOperation.Error}");
            // }
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 异步加载Unity资源
        /// </summary>
        /// <param name="assetName">资源名称或路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        /// <exception cref="GameFrameworkException"></exception>
        public async UniTask<T> LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
            if (defaultPackage == null || !IsInitialize)
            {
                throw new GameFrameworkException("package is null or not initialize.");
            }

            var handle = defaultPackage.LoadAssetAsync<T>(assetName);
            await handle;
            return handle.AssetObject as T;
        }
            
        /// <summary>
        /// 同步加载Unity资源
        /// </summary>
        /// <param name="assetName">资源名称或路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        /// <exception cref="GameFrameworkException"></exception>
        public T LoadAssetSync<T>(string assetName) where T : UnityEngine.Object
        {
            if (defaultPackage == null || !IsInitialize)
            {
                throw new GameFrameworkException("package is null or not initialize.");
            }

            var handle = defaultPackage.LoadAssetSync<T>(assetName);
            return handle.AssetObject as T;
        }
    }
}