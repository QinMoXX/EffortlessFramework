using System.Threading.Tasks;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
using UnityEngine;

namespace Src
{
    public class DownloadProcedure:ProcedureBase
    {
        public DownloadProcedure(IFsm handle) : base(handle)
        {
        }

        protected override void OnInit()
        {
            Debug.Log("DownloadProcedure OnInit");
        }

        protected override void OnEnter()
        {
            Debug.Log("DownloadProcedure OnEnter");
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Debug.Log("DownloadProcedure OnUpdate");
            this.ChangeState<EnterGameProcedure>();
        }

        protected override void OnLeave()
        {
            Debug.Log("DownloadProcedure OnLeave");
        }
    }
}