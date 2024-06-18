using Src.AOT.Framework.Fsm;

namespace Src.AOT.Framework.Procedure
{
    public sealed class ProcedureFsm:FsmBase
    {
        
    }

    public abstract class ProcedureBase : FsmStateBase
    {
        protected ProcedureBase(IFsm handle) : base(handle)
        {
            
        }
        
    } 
}