using System;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;

namespace AOT.Framework.ObjectPool
{
    public partial class ObjectPoolManager:IGameModule
    {
        public sealed class ObjectPool<T>:IObjectPool where T:ObjectBase,new()
        {
            private readonly Dictionary<object, Object<T>> m_ObjectDict; //全部对象字典
            private readonly Dictionary<string, List<Object<T>>> m_ObjectBucket; //对象桶
            private readonly List<Object<T>> m_CachedCanReleaseObjects; //可释放对象缓存
            private readonly List<Object<T>> m_CachedCanToReleaseObjects; //即将释放对象缓存
            private float m_AutoReleaseTime; //自动释放时间
            private readonly float m_AutoReleaseInterval;    //自动释放间隔时间
            private readonly int m_Capacity; //容量
            private readonly bool m_AllowMultiSpawn; //是否允被多次引用
            private readonly string m_Name; //名称
            private readonly int m_Priority; //优先级
            private readonly float m_ExpireTime; //对象过期时间(小于0永不过期，等0立刻过期，大于0间隔时间过期)

            
            /// <summary>
            /// 名称
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 对象类型 继承于<see cref="ObjectBase"/>
            /// </summary>
            public Type Type
            {
                get
                {
                    return typeof(T);
                }
            }

            /// <summary>
            /// 对象池容量，自动清理限制
            /// </summary>
            public int Capacity
            {
                get
                {
                    return  m_Capacity;
                }
            }

            /// <summary>
            /// 对象池对象数量
            /// </summary>
            public int Count
            {
                get
                {
                    return m_ObjectDict.Count;
                }
            }

            /// <summary>
            /// 对象过期时间
            /// </summary>
            public float ExpireTime
            {
                get
                {
                    return  m_ExpireTime;
                }
            }

            /// <summary>
            /// 对象池优先级
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            /// <summary>
            /// 是否允许对象多次获取
            /// 允许多次获取的对象池永远只存在单个对象
            /// </summary>
            public bool AllowMultiSpawn
            {
                get
                {
                    return m_AllowMultiSpawn;
                }
            }

            /// <summary>
            /// 对象池构造函数
            /// </summary>
            /// <param name="name">对象池名称</param>
            /// <param name="priority">对象池优先级</param>
            /// <param name="capacity">对象池容量</param>
            /// <param name="allowMultiSpawn">是否允许对象多次获取</param>
            /// <param name="autoReleaseInterval">对象池自动释放间隔时间</param>
            /// <param name="expireTime">对象过期时间单位秒数</param>
            public ObjectPool(string name, int priority,int capacity ,bool allowMultiSpawn,float autoReleaseInterval, float expireTime)
            {
                m_Name = name;
                m_Priority = priority;
                m_Capacity = capacity;
                m_AllowMultiSpawn = allowMultiSpawn;
                m_ExpireTime = expireTime;
                m_AutoReleaseTime = 0;
                m_AutoReleaseInterval = autoReleaseInterval;
                m_ObjectDict = new Dictionary<object, Object<T>>();
                m_CachedCanReleaseObjects = new List<Object<T>>();
                m_CachedCanToReleaseObjects = new List<Object<T>>();
                m_ObjectBucket = new Dictionary<string, List<Object<T>>>();
            }
            
            public bool HasObject(object target)
            {
                if (target == null)
                {
                    throw new GameFrameworkException("Target is invalid.");
                }
                if (!m_ObjectDict.TryGetValue(target, out var obj))
                {
                    return false;
                }

                return true;
            }

            public void Initialize(int maxCount)
            {
                
            }

            public IObject Spawn(string objectName)
            {
                Object<T> obj = null;
                if (m_ObjectBucket.TryGetValue(objectName, out var objectList))
                {
                    foreach (var internalObject in objectList)
                    {
                        if (internalObject.ReleaseCount <= 0 | m_AllowMultiSpawn)
                        {
                            obj = internalObject;
                            break;
                        }
                    }
                }
                if (obj == null)
                {
                    obj = CreateObject(objectName);
                }
                
                obj.OnSpawn();
                return  obj;
            }

            public Object<T> CreateObject(string objectName)
            {
                if (string.IsNullOrEmpty(objectName))
                {
                    throw  new GameFrameworkException("Object name is invalid.");
                }
                T @object = ReferencePool.Acquire<T>();
                @object.Initialize(objectName);
                Object<T> internalObject = Object<T>.Create(objectName,@object);
                m_ObjectDict.Add(internalObject.Target, internalObject);
                if (!m_ObjectBucket.TryGetValue(objectName, out var objectList))
                {
                    objectList = new List<Object<T>>();
                    m_ObjectBucket.Add(objectName, objectList);
                }
                objectList.Add(internalObject);
                return internalObject;
            }

            public void Despawn(object target)
            {
                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.OnDespawn();
                }
            }

            public void Release(int toReleaseCount)
            {
                if (toReleaseCount < 0 )
                {
                    toReleaseCount = 0;
                }

                DateTime expireTime = DateTime.MinValue;
                if (m_ExpireTime >= 0 & m_ExpireTime < float.MaxValue)
                {
                    expireTime = DateTime.UtcNow.AddSeconds(-m_ExpireTime);
                }
                //释放后重置释放冷却时间
                m_AutoReleaseTime = 0;
                
                GetCanReleaseObjects();
                GetCanToReleaseObjects(expireTime,toReleaseCount);
                if (m_CachedCanToReleaseObjects.Count <= 0)
                {
                    return;
                }

                foreach (var releaseObject in m_CachedCanToReleaseObjects)
                {
                    ReleaseObj(releaseObject);
                }
                m_CachedCanReleaseObjects.Clear();
                m_CachedCanToReleaseObjects.Clear();
            }

            private void ReleaseObj(Object<T> internalObject)
            {
                if (internalObject == null)
                {
                    throw  new GameFrameworkException("Object is invalid.");
                }
                m_ObjectDict.Remove(internalObject.Target);
                if (m_ObjectBucket.TryGetValue(internalObject.Name, out var objectList))
                {
                    objectList.Remove(internalObject);
                }
                internalObject.Release();
                ReferencePool.Release(internalObject);
            }

    

            private void GetCanToReleaseObjects(DateTime expireTime,int toReleaseCount)
            {
                m_CachedCanToReleaseObjects.Clear();
                if (expireTime > DateTime.MinValue)
                {
                    //根据过期时间 释放过期对象
                    for (int i = m_CachedCanReleaseObjects.Count - 1; i >= 0; i--)
                    {
                        if (m_CachedCanReleaseObjects[i].LastUseTime >= expireTime)
                        {
                            m_CachedCanToReleaseObjects.Add(m_CachedCanReleaseObjects[i]);
                            m_CachedCanReleaseObjects.RemoveAt(i);
                        }
                    }
                    toReleaseCount -= m_CachedCanToReleaseObjects.Count;
                }
                //根据数量 补充释放对象
                if (toReleaseCount > 0 && m_CachedCanReleaseObjects.Count > 0)
                {
                    for (int i = m_CachedCanReleaseObjects.Count-1; i >= 0 & toReleaseCount > 0; i--)
                    {
                        m_CachedCanToReleaseObjects.Add(m_CachedCanReleaseObjects[i]);
                        toReleaseCount--;
                    }
                }
            }

            /// <summary>
            /// 获取可以释放的对象，放入可释放缓存列表
            /// </summary>
            private void GetCanReleaseObjects()
            {
                m_CachedCanReleaseObjects.Clear();
                foreach (var item in m_ObjectDict)
                {
                    if (item.Value.ReleaseCount > 0)
                    {
                        continue;
                    }
                    m_CachedCanReleaseObjects.Add(item.Value);
                }
            }

            private Object<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new GameFrameworkException("Target is invalid.");
                }

                Object<T> internalObject = null;
                if (m_ObjectDict.TryGetValue(target, out internalObject))
                {
                    return internalObject;
                }
                return null;
            }

            public void Update(float virtualElapseSeconds, float realElapseSeconds)
            {
                m_AutoReleaseTime += realElapseSeconds;
                if (m_AutoReleaseInterval < 0 | m_AutoReleaseTime < m_AutoReleaseInterval)
                {
                    return;
                }
                //自动释放按照容量释放
                Release(Count - m_Capacity);
            
            }

            public void Destroy()
            {
                m_ObjectDict.Clear();
                m_CachedCanReleaseObjects.Clear();
                m_CachedCanToReleaseObjects.Clear();
            }
        }
    }
}