using System;

namespace AOT.Framework.ObjectPool
{
    public partial class ObjectPoolManager:IGameModule
    {
        public sealed class Object<T>:IObject,IReference where T:ObjectBase
        {
            private string m_Name;
            private T m_Object;
            private DateTime m_LastUseTime;
            private  bool m_IsRelease;
            private int m_ReferenceCount;

            public Object(string mName, T @object)
            {
                m_Name = mName;
                m_Object = @object;
                m_IsRelease = false;
                m_ReferenceCount = 0;
            }

            public Object()
            {
                m_IsRelease = false;
                m_ReferenceCount = 0;
            }

            public static Object<T> Create(string name, T @object)
            {
                Object<T> internalObject = ReferencePool.Acquire<Object<T>>();
                internalObject.m_Name = name;
                internalObject.m_Object = @object;
                return internalObject;
            }

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public object Target
            {
                get
                {
                    return m_Object?.Target;
                }
            }

            public DateTime LastUseTime
            {
                get
                {
                    return  m_LastUseTime;
                }
            }

            public T GetObjectBase()
            {
                return  m_Object;
            }

            public bool IsRelease
            {
                get
                {
                    return  m_IsRelease;
                }
            }

            public int ReleaseCount
            {
                get
                {
                    return   m_ReferenceCount;
                }
            }

            public void OnSpawn()
            {
                m_LastUseTime = DateTime.UtcNow;
                m_ReferenceCount++;
            }

            public void OnDespawn()
            {
                m_LastUseTime = DateTime.UtcNow;
                m_ReferenceCount--;
            }

            public void Release()
            { 
                m_Object.Release();
                ReferencePool.Release(m_Object);
            }

            public void Clear()
            {
                m_ReferenceCount = 0;
                m_IsRelease = true;
                m_Object = null;
                m_Name = null;
            }
        }
    }
}