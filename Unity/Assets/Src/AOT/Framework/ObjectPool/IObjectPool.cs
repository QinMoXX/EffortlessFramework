using System;

namespace AOT.Framework.ObjectPool
{
    public interface IObjectPool
    {
        public string Name { get; }
        
        public Type Type { get; }
        
        public int Capacity { get; }
        
        public float ExpireTime { get; }
        
        public int Priority { get; }
        
        public bool AllowMultiSpawn { get; }

        public bool HasObject(object target);
        
        public int Count { get; }
        
        public void Initialize(int maxCount);
        
        public IObject Spawn();
        
        public void Despawn(object target);
        
        public void Update(float virtualElapse, float realElapse);
        
        public void Destroy();
    }
}