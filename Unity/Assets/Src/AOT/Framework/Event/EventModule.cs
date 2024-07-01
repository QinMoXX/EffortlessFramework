using System;
using System.Collections.Generic;

namespace AOT.Framework.Event
{
    public sealed class EventManager:SingletonInstance<EventManager>,IGameModule
    {
        public short Priority => 2;

        private Dictionary<Type, EventBasis> m_EventBasisDic;
        
        public void Init()
        {
            m_EventBasisDic = new Dictionary<Type, EventBasis>();
        }
        
        /// <summary>
        /// 轮询事件组
        /// </summary>
        /// <param name="virtualElapse"></param>
        /// <param name="realElapse"></param>
        public void Update(float virtualElapse, float realElapse)
        {
            foreach (var eventBasis in m_EventBasisDic.Values)
            {
                eventBasis.Update();
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="onEvent">需要注册的事件方法</param>
        /// <typeparam name="TEventArgs">事件传递的数据类型</typeparam>
        /// <typeparam name="TGroup">注册事件组类型</typeparam>
        public void Register<TEventArgs, TGroup>(UltraEventHandler<TEventArgs> onEvent) where TEventArgs : struct where TGroup : EventBasis
        {
            EventBasis eventBasis = null;
            if (!m_EventBasisDic.TryGetValue(typeof(TGroup),out eventBasis))
            {
                eventBasis = EventBasis.CreatBasis<TGroup>();
                m_EventBasisDic.TryAdd(typeof(TGroup), eventBasis);
            }

            var eventBase = eventBasis.GetOrAddEvent<EventBase<TEventArgs>>();
            eventBase.Register(onEvent);
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="onEvent">需要注销的事件方法</param>
        /// <typeparam name="TEventArgs">事件传递的数据类型</typeparam>
        /// <typeparam name="TGroup">注销事件组类型</typeparam>
        public void UnRegister<TEventArgs, TGroup>(UltraEventHandler<TEventArgs> onEvent) where TEventArgs : struct where TGroup : EventBasis
        {
            if (!m_EventBasisDic.TryGetValue(typeof(TGroup),out EventBasis eventBasis))
            {
                //不存在该事件组
                return;
            }
            var eventBase = eventBasis.GetEvent<EventBase<TEventArgs>>();
            if (eventBase == null)
            {
                //不存在事件类型
                return;
            }
            eventBase.UnRegister(onEvent);
        }

        /// <summary>
        /// 注销所有组的事件
        /// </summary>
        /// <param name="onEvent">需要注销的事件方法</param>
        /// <typeparam name="T">事件类型</typeparam>
        public void UnRegister<TEventArgs>(UltraEventHandler<TEventArgs> onEvent) where TEventArgs : struct
        {
            foreach (var eventBasis in m_EventBasisDic.Values)
            {
                eventBasis.GetEvent<EventBase<TEventArgs>>()?.UnRegister(onEvent);
            }
        }

        /// <summary>
        /// 注销全部事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TGroup"></typeparam>
        public void UnRegisterAll<TGroup>() where TGroup : EventBasis
        {
            if (!m_EventBasisDic.TryGetValue(typeof(TGroup),out EventBasis eventBasis))
            {
                //不存在该事件组
                return;
            }
            eventBasis.RemoveAll();
        }

        /// <summary>
        /// 注销全部事件
        /// </summary>
        public void UnRegisterAll()
        {
            foreach (var eventBasis in m_EventBasisDic.Values)
            {
                eventBasis.RemoveAll();
            }
        }

        /// <summary>
        /// 发送事件。线程安全，事件会在下一帧执行
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件数据</param>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        /// <typeparam name="TGroup">发送事件组类型</typeparam>
        public void Send<TEventArgs, TGroup>(object sender,TEventArgs e) where TEventArgs : struct where TGroup : EventBasis
        {
            if (!m_EventBasisDic.TryGetValue(typeof(TGroup), out EventBasis eventBasis))
            {
                throw new GameFrameworkException(($"EventBasis<{typeof(TGroup).Name}> is not exist."));
            }
            eventBasis.GetEvent<EventBase<TEventArgs>>()?.Send(sender,e);
        }
        
        /// <summary>
        /// 发送事件。线程不安全，事件立即执行
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件数据</param>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        /// <typeparam name="TGroup">发送事件组类型</typeparam>
        public void SendNow<TEventArgs, TGroup>(object sender,TEventArgs e) where TEventArgs : struct where TGroup : EventBasis
        {
            if (!m_EventBasisDic.TryGetValue(typeof(TGroup), out EventBasis eventBasis))
            {
                throw new GameFrameworkException(($"EventBasis<{typeof(TGroup).Name}> is not exist."));
            }
            eventBasis.GetEvent<EventBase<TEventArgs>>()?.SendNow(sender,e);
        }

        /// <summary>
        /// 发送事件，包含全部组。线程安全，事件会在下一帧执行
        /// </summary>
        /// <param name="e"></param>
        /// <typeparam name="TEvent"></typeparam>
        public void Send<TEventArgs>(object sender,TEventArgs e) where TEventArgs : struct
        {
            foreach (var eventBasis in m_EventBasisDic.Values)
            {
                eventBasis.GetEvent<EventBase<TEventArgs>>()?.Send(sender,e);
            }
        }
        
        /// <summary>
        /// 发送事件，包含全部组。线程不安全，立即执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <typeparam name="TEventArgs"></typeparam>
        public void SendNow<TEventArgs>(object sender,TEventArgs e) where TEventArgs : struct
        {
            foreach (var eventBasis in m_EventBasisDic.Values)
            {
                eventBasis.GetEvent<EventBase<TEventArgs>>()?.SendNow(sender,e);
            }
        }

        public void Destroy()
        {
            m_EventBasisDic.Clear();
            m_EventBasisDic = null;
        }
    }
}