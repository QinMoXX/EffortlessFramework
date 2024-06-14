using System;
using System.Collections.Generic;
using UnityEngine;

namespace Src.AOT.Framework.Fsm
{
  public abstract class FsmBase{
  
    
  }

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
    public IFsmState CurrentState { get; }
    
    /// <summary>
    /// 开始有限状态机
    /// </summary>
    /// <typeparam name="TState">初始状态类型</typeparam>
    public void Start<TState>() where TState : IFsmState;

    /// <summary>
    /// 开始有限状态机
    /// </summary>
    /// <param name="stateType">初始状态类型</param>
    public void Start(Type stateType);
    
    /// <summary>
    /// 是否存在有限状态机状态
    /// </summary>
    /// <typeparam name="TState">检查的有限状态机类型</typeparam>
    /// <returns></returns>
    public bool HasState<TState>() where TState : IFsmState;

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
    public IFsmState GetState<TState>() where TState : IFsmState;

    /// <summary>
    /// 获取有限状态机状态
    /// </summary>
    /// <param name="stateType">获取的状态类型</param>
    /// <returns>获取的状态</returns>
    public IFsmState GetState(Type stateType);

    /// <summary>
    /// 获取所有状态机状态
    /// </summary>
    /// <returns>状态机所有状态</returns>
    public IFsmState[] GetAllStates();
    
    internal void Update(float elapseSeconds, float realElapseSeconds);

    internal void Shutdown();
  }

  public interface IFsmState
  {
    void OnInit();
    void OnEnter();
    void OnUpdate(float elapseSeconds, float realElapseSeconds);
    void OnLeave();
    void OnDestroy();
    void ChangeState<TState>() where TState:IFsmState;
    void ChangeState(Type stateType);
  }

  public sealed class Fsm<T>
  {
    
  }
}