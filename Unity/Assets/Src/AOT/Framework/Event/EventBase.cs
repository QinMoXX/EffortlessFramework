using System;
using System.Collections;
using System.Collections.Generic;

namespace AOT.Framework.Event
{
    /// <summary>
    /// 可注销
    /// </summary>
    public interface IUnRegister
    {
        void UnRegister();
    }

    /// <summary>
    /// 事件注销器（为匿名方法提供外部注销方式）,释放后务必不要再次使用
    /// </summary>
    public sealed class EventUnRegister:IUnRegister,IReference
    {
        //委托对象
        public Action OnUnRegister { get; set; }
        
        public void UnRegister()
        {
            OnUnRegister?.Invoke();
            OnUnRegister = null;
            ReferencePool.Release(this);
        }

        public void Clear()
        {
            OnUnRegister = null;
        }
    }
    /// <summary>
    /// 单一参数泛型事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EventBase<TEventArgs> : IEvent where TEventArgs:struct
    {
        private LinkedList<UltraEventHandler<TEventArgs>> m_OnEvent;
        private Queue<EventNode<TEventArgs>> m_EventNodeQueue;
        
        public  EventBase()
        {
            m_OnEvent = new LinkedList<UltraEventHandler<TEventArgs>>();
            m_EventNodeQueue = new Queue<EventNode<TEventArgs>>();
        }

        public IUnRegister Register(UltraEventHandler<TEventArgs> onEvent)
        {
            m_OnEvent.AddLast(onEvent);
            EventUnRegister unRegisterEvent = ReferencePool.Acquire<EventUnRegister>();
            unRegisterEvent.OnUnRegister = () => { this.UnRegister(onEvent); };
            return unRegisterEvent;
        }

        public void UnRegister(UltraEventHandler<TEventArgs> onEvent)
        {
            m_OnEvent.Remove(onEvent);
        }

        /// <summary>
        /// 立即触发事件，这个操作不是线程安全的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SendNow(object sender,TEventArgs e)
        {
            if (m_OnEvent.Count > 0)
            {
                var currentNode = m_OnEvent.Last;
                while (currentNode != null)
                {
                    currentNode.Value.Invoke(sender,e);
                    currentNode = currentNode.Previous;
                }
            }
        }
        
        /// <summary>
        /// 线程安全的事件触发，在下一帧触发
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e"></param>
        public void Send(object sender, TEventArgs e)
        {
            EventNode<TEventArgs> eventNode = EventNode<TEventArgs>.Create(sender, e);
            lock (m_EventNodeQueue)
            {
                m_EventNodeQueue.Enqueue(eventNode);
            }
        }
        
        
        public int Count
        {
            get => m_OnEvent?.Count ?? 0;
        }

        public void Clear()
        {
            m_OnEvent.Clear();
            m_OnEvent = null;
            lock (m_EventNodeQueue)
            {
                m_EventNodeQueue.Clear();
                m_EventNodeQueue = null;
            }
        }

        /// <summary>
        /// 轮询事件
        /// </summary>
        public void Update()
        {
            lock (m_EventNodeQueue)
            {
                while (m_EventNodeQueue.Count > 0)
                {
                    EventNode<TEventArgs> eventNode = m_EventNodeQueue.Dequeue();
                    SendNow(eventNode.Sender,eventNode.EventArgs);
                    ReferencePool.Release(eventNode);
                }
            }
        }
    }
}