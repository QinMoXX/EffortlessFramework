using System;
using System.Collections.Generic;

namespace AOT.Framework.Event
{
    /// <summary>
    /// 事件组基类
    /// </summary>
    public abstract class EventBasis:IDisposable
    {

        //类型字典
        private Dictionary<Type, IEvent> m_typeEventDic = new Dictionary<Type, IEvent>();

        /// <summary>
        /// 添加事件类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddEvent<T>() where T : IEvent
        {
            m_typeEventDic.TryAdd(typeof(T),Activator.CreateInstance<T>());
        }

        /// <summary>
        /// 获取事件类型
        /// </summary>
        /// <typeparam name="T">EventBase <see cref="EventBase"/></typeparam>
        /// <returns></returns>
        public T GetEvent<T>() where T : class, IEvent
        {
            IEvent e = null;

            if (m_typeEventDic.TryGetValue(typeof(T), out e))
            {
                return e as T;
            }
            return null;
        }
        
        /// <summary>
        /// 获取或添加事件类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetOrAddEvent<T>() where T : IEvent
        {
            var eType = typeof(T);
            if (m_typeEventDic.TryGetValue(eType, out var e))
            {
                return (T)e;
            }

            var t = Activator.CreateInstance<T>();
            m_typeEventDic.Add(eType, t);
            return t;
        }

        /// <summary>
        /// 移除事件类型
        /// </summary>
        /// <typeparam name="T">要移除的事件类型</typeparam>
        public void Remove<T>() where T : IEvent
        {
            Type eventType = typeof(T);
            if (!m_typeEventDic.TryGetValue(eventType, out var eEvent))
            {
                return;
            }
            eEvent.Clear();
            m_typeEventDic.Remove(eventType);
        }

        /// <summary>
        /// 移除所有事件类型
        /// </summary>
        public void RemoveAll()
        {
            foreach (var evetBase in m_typeEventDic.Values)
            {
                evetBase.Clear();
            }
            m_typeEventDic.Clear();
        }

        /// <summary>
        /// 事件类型总量
        /// </summary>
        /// <returns></returns>
        public int Count => m_typeEventDic.Count;

        /// <summary>
        /// 事件类型是否存在
        /// </summary>
        /// <returns></returns>   
        public bool Contains<T>() where T : IEvent
        {
            return m_typeEventDic.ContainsKey(typeof(T));
        }
        
        /// <summary>
        /// 事件类型是否存在
        /// </summary>
        /// <param name="type">事件类型<see cref="IEvent"/>></param>
        /// <returns></returns>
        public bool Contains(Type type)
        {
            return m_typeEventDic.ContainsKey(type);
        }

        /// <summary>
        /// 轮询事件组
        /// </summary>
        public void Update()
        {
            foreach (var value in m_typeEventDic.Values)
            {
                value.Update();
            }
        }
        
        /// <summary>
        /// 创建事件组
        /// </summary>
        /// <typeparam name="T">事件组类型</typeparam>
        /// <returns></returns>
        public static T CreatBasis<T>() where T:EventBasis
        {
            return Activator.CreateInstance<T>();
        }
        
        public void Dispose()
        {
            m_typeEventDic.Clear();
            m_typeEventDic = null;
        }
    }
}