using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Framework;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using Src.AOT.Framework.Resource;
using UnityEngine;

namespace Src
{
    public class VersionCheckProcedure:ProcedureBase
    {
        private string packageVersion; 
        public VersionCheckProcedure(IFsm handle) : base(handle)
        {
        }

        
        protected override void OnInit()
        {
            
        }

        protected override async void OnEnter()
        {
            // packageVersion = string.Empty;
            // packageVersion = await GameEntry.GetModule<ResourceManager>().GetUpdatePackageVersion();
            Debug.Log("VersionCheckProcedure OnEnter");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Debug.Log("VersionCheckProcedure OnUpdate");
            // if (packageVersion == string.Empty)
            // {
            //     return;
            // }
            this.ChangeState<DownloadProcedure>();
        }

        protected override void OnLeave()
        {
            Debug.Log("VersionCheckProcedure OnLeave");
        }
    }
}