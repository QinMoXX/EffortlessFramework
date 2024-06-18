using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Framework;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using Src.AOT.Framework.Resource;

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
            packageVersion = string.Empty;
            packageVersion = await GameEntry.GetModule<ResourceManager>().GetUpdatePackageVersion();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (packageVersion == string.Empty)
            {
                return;
            }
            
        }

        protected override void OnLeave()
        {
            
        }
    }
}