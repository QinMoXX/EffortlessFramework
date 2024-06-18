using System;

namespace Src.AOT.Framework.Fsm
{
    public abstract class FsmStateBase
    {
        protected IFsm m_fsmHandle;
        protected internal FsmStateBase(IFsm handle)
        {
            this.m_fsmHandle = handle;
        }

        protected internal abstract void OnInit();

        protected internal abstract void OnEnter();

        protected internal abstract void OnUpdate(float elapseSeconds, float realElapseSeconds);

        protected internal abstract void OnLeave();

        protected virtual void OnDestroy()
        {
            this.m_fsmHandle = null;
        }

        protected void ChangeState<TState>() where TState : FsmStateBase
        {
            if (this.m_fsmHandle == null)
            {
                throw new Exception("Fsm is invalid");
            }
            this.m_fsmHandle.ChangeState<TState>();
        }

        protected void ChangeState(Type stateType)
        {
            if (this.m_fsmHandle == null)
            {
                throw new Exception("Fsm is invalid");
            }
            this.m_fsmHandle.ChangeState(stateType);
        }
    }
    
}