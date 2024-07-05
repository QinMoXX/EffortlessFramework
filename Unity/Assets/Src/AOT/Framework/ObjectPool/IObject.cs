using System;

namespace AOT.Framework.ObjectPool
{
    public interface IObject
    {
        public string Name { get; }
        public object Target { get; }
        
        public DateTime LastUseTime { get; }
        
        public bool IsRelease { get; }
        
        public int ReleaseCount { get; }
        
        public void OnSpawn();
        
        public void OnDespawn();
        
        public void Release();
    }
}