using System.Threading.Tasks;
using Framework;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using Src.AOT.Framework.Resource;
using UnityEngine;

namespace Src
{
    public class EnterGameProcedure:ProcedureBase
    {
        public EnterGameProcedure(IFsm handle) : base(handle)
        {
            
        }

        protected override void OnInit()
        {
            Debug.Log("EnterGameProcedure OnInit");
        }

        protected override void OnEnter()
        {
            Debug.Log("EnterGameProcedure OnEnter");
            //进入热更新脚本逻辑，Yooasset下载核对完成后加载后面package
            var package = GameEntry.GetModule<ResourceManager>().defaultPackage;
            //加载原生打包的dll文件
            RawFileOperationHandle handlefiles = package.LoadRawFileAsync(HotDllName);
            await handlefiles.ToUniTask();

#if !UNITY_EDITOR
		//非编辑器下直接加载yooasset中的dll文件
      	byte[] dllBytes = handlefiles.GetRawFileData();
      //加载dll，完成dll热更新
       System.Reflection.Assembly.Load(dllBytes);
#endif
            //完成dll加载后，即可进行加载热更新预制体，通过实例化挂载了热更新脚本的预制体直接进入到热更新层的逻辑
            AssetOperationHandle handle = package.LoadAssetAsync<GameObject>("HotFix_Import");
            await handle.ToUniTask();
            GameObject go = handle.InstantiateSync();
            Debug.Log($"Prefab name is {go.name}"); 作者：我家的柯基叫团团 https://www.bilibili.com/read/cv24251059/ 出处：bilibili
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Debug.Log("EnterGameProcedure OnUpdate");
        }

        protected override void OnLeave()
        {
            Debug.Log("EnterGameProcedure OnLeave");
        }
    }
}