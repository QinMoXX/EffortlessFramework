using AOT.Framework.Debug;
using Cysharp.Threading.Tasks;
using YooAsset;


namespace AOT.Framework.Resource
{
    public partial class ResourceManager:IGameModule,IResourceManager
    {
        private ResourceRunningMode m_mode = ResourceRunningMode.ONLINE;
        public short Priority => 1;

        public string packageVersion;
        public const string DEFAULT_PACKAGE_NAME = "DefaultPackage";

        public ResourcePackage DefaultPackage { get;private set; }
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
            // 先销毁资源包
            var package = YooAssets.GetPackage("DefaultPackage");
            package.DestroyAsync();
        }

        public void Init()
        {
            packageVersion = string.Empty;
        }

        public async UniTask Initialize()
        {
            // 初始化资源系统
            YooAssets.Initialize();
            DefaultPackage = YooAssets.TryGetPackage(DEFAULT_PACKAGE_NAME);
            if (DefaultPackage == null)
            {
                // 创建默认的资源包
                DefaultPackage = YooAssets.CreatePackage("DefaultPackage");
            }
            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(DefaultPackage);

            if (m_mode == ResourceRunningMode.STANDALONE)
            {
                //单机模式运行
                var buildinFileSystem = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                var initParameters = new OfflinePlayModeParameters();
                initParameters.BuildinFileSystemParameters = buildinFileSystem;
                var initOperation = DefaultPackage.InitializeAsync(initParameters);
                await initOperation.ToUniTask();
                if (initOperation.Status == EOperationStatus.Succeed)
                {
                    IsInitialize = true;
                    EDebug.Log("STANDALONE ResourceManager Initialize Success.");
                }
                else
                {
                    IsInitialize = false;
                    EDebug.Log("STANDALONE ResourceManager Initialize Fail.");
                }
            }
            else if (m_mode == ResourceRunningMode.ONLINE)
            {
                EDebug.Log("ONLINE ResourceManager Initialize.");
                await InitializeYooAsset(DefaultPackage);
            }
#if UNITY_EDITOR
            else if(m_mode == ResourceRunningMode.EDITOR)
            {
                //编辑器模式运行
                //注意：如果是原生文件系统选择EDefaultBuildPipeline.RawFileBuildPipeline
                var buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline; 
                var simulateBuildResult = EditorSimulateModeHelper.SimulateBuild(buildPipeline, "DefaultPackage");
                var editorFileSystem = FileSystemParameters.CreateDefaultEditorFileSystemParameters(simulateBuildResult);
                EditorSimulateModeParameters initParameters = new EditorSimulateModeParameters();
                initParameters.EditorFileSystemParameters = editorFileSystem;
                var initOperation = DefaultPackage.InitializeAsync(initParameters);
                await initOperation.ToUniTask();
                if (initOperation.Status == EOperationStatus.Succeed)
                {
                    EDebug.Log("EDITOR ResourceManager Initialize Success.");
                    IsInitialize = true;
                }
                else
                {
                    EDebug.Log("EDITOR ResourceManager Initialize Fail.");
                    IsInitialize = false;
                }
            }
#endif

            
        }
        
        private UniTask InitializeYooAsset(ResourcePackage package)
        {
            // string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";
            // string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";
            // IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            // var cacheFileSystem = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            // var buildinFileSystem = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();   
            // var initParameters = new HostPlayModeParameters();
            // initParameters.BuildinFileSystemParameters = buildinFileSystem; 
            // initParameters.CacheFileSystemParameters = cacheFileSystem;
            // var initOperation = package.InitializeAsync(initParameters);
            // await initOperation;
            // if (initOperation.Status == EOperationStatus.Succeed)
            //     IsInitialize = true;
            // else
            //     IsInitialize = false;
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
            if (DefaultPackage == null || !IsInitialize)
            {
                throw new GameFrameworkException("package is null or not initialize.");
            }

            var handle = DefaultPackage.LoadAssetAsync<T>(assetName);
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
            if (DefaultPackage == null || !IsInitialize)
            {
                throw new GameFrameworkException("package is null or not initialize.");
            }

            var handle = DefaultPackage.LoadAssetSync<T>(assetName);
            return handle.AssetObject as T;
        }
    }
}