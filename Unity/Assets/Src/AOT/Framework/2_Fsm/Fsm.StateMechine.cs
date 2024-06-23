using System;
using System.Collections.Generic;
using System.Linq;
using Src.AOT.Framework.Procedure;

namespace Src.AOT.Framework.Fsm
{
    public interface IFsm
    {
      /// <summary>
      /// 获取状态机名称
      /// </summary>
      public string Name { get; }
      
      /// <summary>
      /// 获取有限状态机中状态数量
      /// </summary>
      public int Count { get; }
      
      /// <summary>
      /// 获取有限状态机运行状态
      /// </summary>
      public bool IsRunning { get; }

      /// <summary>
      /// 获取当前执行状态
      /// </summary>
      public FsmStateBase CurrentState { get; }
      
      /// <summary>
      /// 开始有限状态机
      /// </summary>
      /// <typeparam name="TState">初始状态类型</typeparam>
      public void Start<TState>() where TState : FsmStateBase;

      /// <summary>
      /// 开始有限状态机
      /// </summary>
      /// <param name="stateType">初始状态类型</param>
      public void Start(Type stateType);

      /// <summary>
      /// 停止状态机运行
      /// </summary>
      public void Stop();
      
      /// <summary>
      /// 是否存在有限状态机状态
      /// </summary>
      /// <typeparam name="TState">检查的有限状态机类型</typeparam>
      /// <returns></returns>
      public bool HasState<TState>() where TState : FsmStateBase;

      /// <summary>
      /// 是否存在有限状态机状态
      /// </summary>
      /// <param name="stateType">检查的有限状态机类型</param>
      /// <returns></returns>
      public bool HasState(Type stateType);
      
      /// <summary>
      /// 获取有限状态机状态
      /// </summary>
      /// <typeparam name="TState">获取的状态类型</typeparam>
      /// <returns>获取的状态</returns>
      public TState GetState<TState>() where TState : FsmStateBase;

      /// <summary>
      /// 获取有限状态机状态
      /// </summary>
      /// <param name="stateType">获取的状态类型</param>
      /// <returns>获取的状态</returns>
      public FsmStateBase GetState(Type stateType);

      /// <summary>
      /// 获取所有状态机状态
      /// </summary>
      /// <returns>状态机所有状态</returns>
      public FsmStateBase[] GetAllStates();
      
      /// <summary>
      /// 状态机轮询
      /// </summary>
      /// <param name="elapseSeconds">逻辑流逝时间</param>
      /// <param name="realElapseSeconds">真实流逝时间</param>
      internal void Update(float elapseSeconds, float realElapseSeconds);

      /// <summary>
      /// 关闭状态机
      /// </summary>
      internal void Shutdown();

      /// <summary>
      /// 切换状态机状态
      /// </summary>
      /// <typeparam name="TState">指定切换状态类型</typeparam>
      internal void ChangeState<TState>() where TState : FsmStateBase;

      /// <summary>
      /// 切换状态机状态
      /// </summary>
      /// <param name="stateType">指定切换状态类型</param>
      internal void ChangeState(Type stateType);
    }

    public abstract class FsmBase:IFsm
    {
      private readonly Dictionary<Type, FsmStateBase> m_States;
      private FsmStateBase m_CurrentState;
      
      private string m_Name;
      public string Name
      {
        get
        {
          return m_Name;
        }
        protected set
        {
          m_Name = value ?? string.Empty;
        }
      }
      
      
      public int Count => m_States.Count;
      public bool IsRunning => m_CurrentState != null;
      public FsmStateBase CurrentState => m_CurrentState;

      public FsmBase()
      {
        m_States = new Dictionary<Type, FsmStateBase>();
        m_CurrentState = null;
      }
      
      
      public void Start<TState>() where TState : FsmStateBase
      {
        if (IsRunning)
        {
          throw new Exception("Fsm is Running, can not start again.");
        }

        FsmStateBase state = GetState<TState>();
        if (state == null)
        {
          throw new Exception($"Fsm {m_Name} can not start state '{typeof(TState).FullName} which is not exist");
        }

        m_CurrentState = state;
        m_CurrentState.OnEnter();
      }

      public void Start(Type stateType)
      {
        if (IsRunning)
        {
          throw new Exception("Fsm is Running, can not start again.");
        }

        FsmStateBase state = GetState(stateType);
        if (state == null)
        {
          throw new Exception($"Fsm {m_Name} can not start state '{stateType.FullName} which is not exist");
        }

        m_CurrentState = state;
        m_CurrentState.OnEnter();
      }

      public void Stop()
      {
        m_CurrentState.OnLeave();
        m_CurrentState = null;
      }

      public bool HasState<TState>() where TState : FsmStateBase
      {
        return HasState(typeof(TState));
      }

      public bool HasState(Type stateType)
      {
        if (m_States.Count == 0)
        {
          return false;
        }

        if (!m_States.ContainsKey(stateType))
        {
          return false;
        }
        return true;
      }

      public TState GetState<TState>() where TState : FsmStateBase
      {
        return GetState(typeof(TState)) as TState;
      }

      public FsmStateBase GetState(Type stateType)
      {
        if (m_States.TryGetValue(stateType, out FsmStateBase state))
        {
          return state;
        }
        return null;
      }

      public FsmStateBase[] GetAllStates()
      {
        return m_States.Values.ToArray();
      }
   
      void IFsm.Update(float elapseSeconds, float realElapseSeconds)
      {
        if (m_CurrentState == null)
        {
          return;
        }
        m_CurrentState.OnUpdate(elapseSeconds, realElapseSeconds);
      }

      void IFsm.Shutdown()
      {
        Stop();
        //TODO:回收状态机
      }

      void IFsm.ChangeState<TState>()
      {
        (this as IFsm).ChangeState(typeof(TState));
      }

      void IFsm.ChangeState(Type stateType)
      {
        if (m_CurrentState == null)
        {
          throw new Exception("Current state is invalid.");
        }

        FsmStateBase state = null;
        if (!m_States.TryGetValue(stateType, out state))
        {
          throw new Exception($"Fsm '{m_Name} can not change state to '{stateType.FullName}' which is not exist");
        }
        
        m_CurrentState.OnLeave();
        m_CurrentState = state;
        m_CurrentState.OnEnter();
      }

      public static FsmBase Create<T>(string name, params Type[] stateTypes) where T:FsmBase
      {
        if (stateTypes == null || stateTypes.Length<1)
        {
          throw new Exception("Fsm states is invalid");
        }

        FsmBase fsm = Activator.CreateInstance<T>();
        fsm.Name = name;
        foreach (var stateType in stateTypes)
        {
          if (stateType == null)
          {
            throw new Exception("FSM states is invalid.");
          }

          if (!typeof(FsmStateBase).IsAssignableFrom(stateType))
          {
            throw new Exception("Fsm state type is invalid.");
          }

          if (fsm.m_States.ContainsKey(stateType))
          {
            throw new Exception($"Fsm '{name}' state '{stateType.FullName}' is already exist.");
          }

          FsmStateBase state = Activator.CreateInstance(stateType, fsm) as FsmStateBase;
          fsm.m_States.Add(stateType, state);
          state.OnInit();
        }

        return fsm;
      }
    }
    
}