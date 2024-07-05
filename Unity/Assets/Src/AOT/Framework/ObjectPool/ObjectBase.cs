using System;

namespace AOT.Framework.ObjectPool
{
    /// <summary>
    /// 对象基类。
    /// </summary>
    public abstract class ObjectBase : IReference
    {
        private string m_Name;
        private object m_Target;

        /// <summary>
        /// 初始化对象基类的新实例。
        /// </summary>
        public ObjectBase()
        {
            m_Name = null;
            m_Target = null;
        }

        /// <summary>
        /// 获取对象名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取对象。
        /// </summary>
        public object Target
        {
            get
            {
                return m_Target;
            }
            protected set
            {
                m_Target = value;
            }
        }

        /// <summary>
        /// 初始化对象基类。
        /// </summary>
        /// <param name="name">对象名称。</param>
        protected internal virtual void Initialize(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw  new GameFrameworkException("Name is invalid.");
            }
            m_Name = name;
        }

        /// <summary>
        /// 清理对象基类。
        /// </summary>
        public virtual void Clear()
        {
            m_Name = null;
            m_Target = null;
        }

        /// <summary>
        /// 获取对象时的事件。
        /// </summary>
        protected internal virtual void OnSpawn()
        {
        }

        /// <summary>
        /// 回收对象时的事件。
        /// </summary>
        protected internal virtual void OnDespawn()
        {
            
        }

        /// <summary>
        /// 释放对象。
        /// </summary>
        protected internal abstract void Release();
    }
}
