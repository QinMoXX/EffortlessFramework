
using System;
using System.Collections.Generic;

namespace AOT.Framework.ObjectPool
{
    public partial class ObjectPoolManager:IGameModule,IObjectPoolManager
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const int DefaultPriority = 0;
        public short Priority => 2;

        private readonly Dictionary<TypeNamePair, IObjectPool> m_ObjectPools;

        public ObjectPoolManager()
        {
            m_ObjectPools = new Dictionary<TypeNamePair, IObjectPool>();
        }
        
        public void Init()
        {
            
        }

        public void Update(float virtualElapse, float realElapse)
        {
            foreach (var objectPool in m_ObjectPools.Values)
            {
                objectPool.Update(virtualElapse, realElapse);
            }
        }

        public void Destroy()
        {
            foreach (var objectPool in m_ObjectPools.Values)
            {
                objectPool.Destroy();
            }
            m_ObjectPools.Clear();
        }


        public IObjectPool GetObjectPool(Type objectType, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw  new GameFrameworkException("Object Pool name is null or empty");
            }
            
            TypeNamePair typeNamePair = new TypeNamePair(objectType, name);
            if (!m_ObjectPools.TryGetValue(typeNamePair, out IObjectPool objectPool))
            {
                return null;
            }
            return  objectPool;
        }

        public ObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Object Pool name is null or empty");
            }
            return GetObjectPool(typeof(T), name) as ObjectPool<T>;
        }

        public ObjectPool<T> CreateObjectPool<T>(string name, bool allowMultiSpawn, int priority = DefaultPriority,
            int capacity = DefaultCapacity, float autoReleaseInterval = DefaultExpireTime,
            float expireTime = DefaultExpireTime) where T : ObjectBase, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Object Pool name is null or empty");
            }
            
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), name);
            if (m_ObjectPools.ContainsKey(typeNamePair))
            {
                throw new GameFrameworkException("Object Pool name is already exist");
            }
            ObjectPool<T> objectPool = new ObjectPool<T>(name, priority, capacity, allowMultiSpawn, autoReleaseInterval, expireTime);
            m_ObjectPools.Add(typeNamePair, objectPool);
            return  objectPool;
        }

        public IObjectPool CreateObjectPool(Type objectType, string name, bool allowMultiSpawn, int priority = DefaultPriority,
            int capacity = DefaultCapacity, float autoReleaseInterval = DefaultExpireTime,
            float expireTime = DefaultExpireTime)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Object Pool name is null or empty");
            }
            if (objectType == null)
            {
                throw new GameFrameworkException("Object type is invalid");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw  new GameFrameworkException(Utility.String.Format("Object type '{0}' is invalid.", objectType));
            }
            TypeNamePair typeNamePair = new TypeNamePair(objectType, name);
            if (m_ObjectPools.ContainsKey(typeNamePair))
            {
                throw new GameFrameworkException("Object Pool name is already exist");
            }
            Type objectPoolType = typeof(ObjectPool<>).MakeGenericType(objectType);
            IObjectPool objectPool = (IObjectPool)Activator.CreateInstance(objectPoolType, name, priority, capacity, allowMultiSpawn, autoReleaseInterval, expireTime);
            m_ObjectPools.Add(typeNamePair, objectPool);
            return  objectPool;
        }

        public void DestroyObjectPool<T>(string name) where T : ObjectBase, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Object Pool name is null or empty");
            }

            InternalDestroyObjectPool(new TypeNamePair(typeof(T), name));
        }

        public void DestroyObjectPool(Type objectType, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Object Pool name is null or empty");
            }
            if (objectType == null)
            {
                throw new GameFrameworkException("Object type is invalid");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw  new GameFrameworkException(Utility.String.Format("Object type '{0}' is invalid.", objectType));
            }
            InternalDestroyObjectPool(new TypeNamePair(objectType, name));
        }
        
        private bool InternalDestroyObjectPool(TypeNamePair typeNamePair)
        {
            IObjectPool objectPool = null;
            if (m_ObjectPools.TryGetValue(typeNamePair, out objectPool))
            {
                objectPool.Destroy();
                return m_ObjectPools.Remove(typeNamePair);
            }

            return false;
        }


        public bool HasObjectPool<T>(string name) where T : ObjectBase, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return m_ObjectPools.ContainsKey(new TypeNamePair(typeof(T), name));
        }

        public bool HasObjectPool(Type objectType, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (objectType == null)
            {
                return false;
            }
            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                return false;
            }
            return m_ObjectPools.ContainsKey(new TypeNamePair(objectType, name));
        }
    }
}