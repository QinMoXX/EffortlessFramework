using Cysharp.Threading.Tasks;
using Framework;
using Src.AOT.Framework.Debug;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using Src.AOT.Framework.Resource;
using UnityEngine;
using YooAsset;

namespace Src
{
    public sealed class VersionCheckProcedure:ProcedureBase
    {
        private string packageVersion; 
        public VersionCheckProcedure(IFsm handle) : base(handle)
        {
        }

        
        protected internal override void OnInit()
        {
            
        }

        protected internal override async void OnEnter()
        {
            var resourceManager = GameEntry.GetModule<ResourceManager>();
            await resourceManager.Initialize();
            packageVersion = string.Empty;
            packageVersion = await GetUpdatePackageVersion();
            EDebug.Log("VersionCheckProcedure OnEnter");
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Debug.Log("VersionCheckProcedure OnUpdate");
            if (packageVersion == string.Empty || GameEntry.GetModule<ResourceManager>().defaultPackage.InitializeStatus != EOperationStatus.Succeed)
            {
                return;
            }
            GameEntry.GetModule<ResourceManager>().packageVersion = packageVersion;
            this.ChangeState<DownloadProcedure>();
        }

        protected internal override void OnLeave()
        {
            Debug.Log("VersionCheckProcedure OnLeave");
        }
        
        /// <summary>
        /// 获取包版本
        /// </summary>
        /// <returns></returns>
        public async UniTask<string> GetUpdatePackageVersion()
        {
            var resourceManager = GameEntry.GetModule<ResourceManager>();
            var package = resourceManager.defaultPackage;
            var operation = package.UpdatePackageVersionAsync();
            await operation.ToUniTask();
            if (operation.Status != EOperationStatus.Succeed)
            {
                //资源版本获取失败
                Debug.LogError(operation.Error);
                return string.Empty;
            }
            //更新成功
            string packageVersion = operation.PackageVersion;
            return packageVersion;
        }
    }
}