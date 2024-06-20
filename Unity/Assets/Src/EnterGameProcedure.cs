using System.Threading.Tasks;
using Src.AOT.Framework.Fsm;
using Src.AOT.Framework.Procedure;
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