using System.Threading.Tasks;
using AOT.Framework;
using Cysharp.Threading.Tasks;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
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
            if (packageVersion == string.Empty || !GameEntry.GetModule<ResourceManager>().IsInitialize)
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
            var package = resourceManager.DefaultPackage;
            var operation = package.RequestPackageVersionAsync();
            await operation.ToUniTask();
            if (operation.Status != EOperationStatus.Succeed)
            {
                //资源版本获取失败
                Debug.LogError(operation.Error);
                return string.Empty;
            }
            //更新成功
            return operation.PackageVersion;
        }
    }
}