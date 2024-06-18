using Cysharp.Threading.Tasks;
using Framework;
using UnityEngine;
using YooAsset;


namespace Src.AOT.Framework.Resource
{
    public partial class ResourceManager:IGameModule
    {
        internal ResourceRunningMode Mode = ResourceRunningMode.EDITOR;
        public short Priority => 1;

        public ResourcePackage defaultPackage;
        public void Update(float virtualElapse, float realElapse)
        {
            
        }

        public void Destroy()
        {
            YooAssets.Destroy();
        }

        public void Init()
        {
            // 初始化资源系统
            YooAssets.Initialize();
            // 创建默认的资源包
            defaultPackage = YooAssets.CreatePackage("DefaultPackage");
            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(defaultPackage);
            if (Mode == ResourceRunningMode.STANDALONE)
            {
                InitializeYooAssetForStandaloneMode(defaultPackage);
            }
            else if (Mode == ResourceRunningMode.ONLINE)
            {
                InitializeYooAsset(defaultPackage);
            }
#if UNITY_EDITOR
            else if(Mode == ResourceRunningMode.EDITOR)
            {
                InitializeYooAssetForEditorMode(defaultPackage);
            }
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// 编辑器模式下运行
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private UniTask InitializeYooAssetForEditorMode(ResourcePackage package)
        {
            var initParameters = new EditorSimulateModeParameters();
            var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
            initParameters.SimulateManifestFilePath  = simulateManifestFilePath;
            return package.InitializeAsync(initParameters).ToUniTask();
        } 
#endif

        /// <summary>
        /// 单机模式运行，需要构建资源包
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private UniTask InitializeYooAssetForStandaloneMode(ResourcePackage package)
        {
            var initParameters = new OfflinePlayModeParameters();
            return package.InitializeAsync(initParameters).ToUniTask();
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

        public async UniTask<string> GetUpdatePackageVersion()
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            var operation = package.UpdatePackageVersionAsync();
            await operation;
            if (operation.Status == EOperationStatus.Succeed)
            {
                //更新成功
                string packageVersion = operation.PackageVersion;
                return packageVersion;
            }

            //更新失败
            Debug.LogError(operation.Error);
            return string.Empty;
        }
            
    }
}