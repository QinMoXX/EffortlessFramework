using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AOT.Framework;
using Cysharp.Threading.Tasks;
using HybridCLR;
using AOT.Framework.Debug;
using AOT.Framework.Fsm;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using UnityEngine;

namespace Src
{
    public class EnterGameProcedure:ProcedureBase
    {
        private const string HotDllName = "HotUpdate";
        private IEntry hotUpdateMain;
        
        public EnterGameProcedure(IFsm handle) : base(handle)
        {
            
        }

        protected internal override void OnInit()
        {
            EDebug.Log("EnterGameProcedure OnInit");
        }

        protected internal override async void OnEnter()
        {
            EDebug.Log("EnterGameProcedure OnEnter");
            // 先补充元数据
             await LoadMetadataForAOTAssemblies();
             //进入热更新脚本逻辑，Yooasset下载核对完成后加载后面package
#if !UNITY_EDITOR
             // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
             var package = GameEntry.GetModule<ResourceManager>().defaultPackage;
             //加载原生打包的dll文件
             var handlefiles = package.LoadAssetSync<TextAsset>(HotDllName + ".dll");
             await handlefiles;
             TextAsset assetObject = handlefiles.AssetObject as TextAsset;
             byte[] dllBytes = assetObject.bytes;
             Assembly hotUpdateAss = Assembly.Load(dllBytes);
#else
             // Editor下无需加载，直接查找获得HotUpdate程序集
             Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == HotDllName);
#endif
              //完成dll加载
              Type type = hotUpdateAss.GetType("HotUpdateMain");
              hotUpdateMain = Activator.CreateInstance(type) as IEntry;
              hotUpdateMain?.Entry();
        }
        
        private async UniTask LoadMetadataForAOTAssemblies()
        {
            List<string> aotDllList = new List<string>
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll", // 如果使用了Linq，需要这个
                // "Newtonsoft.Json.dll", 
                // "protobuf-net.dll",
            };
            var package = GameEntry.GetModule<ResourceManager>().defaultPackage;
            foreach (var aotDllName in aotDllList)
            {
                var handle = package.LoadAssetSync<TextAsset>(aotDllName);
                await handle;
                TextAsset assetObject = handle.AssetObject as TextAsset;
                LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(assetObject.bytes, HomologousImageMode.SuperSet);
                EDebug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
            }
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            EDebug.Log("EnterGameProcedure OnUpdate");
        }

        protected internal override void OnLeave()
        {
            EDebug.Log("EnterGameProcedure OnLeave");
            hotUpdateMain?.Exit();
        }
    }
}