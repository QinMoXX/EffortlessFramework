using System;

namespace AOT.Framework.ObjectPool
{
    public interface IObjectPoolManager
    {
        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        public IObjectPool GetObjectPool(Type objectType, string name);
        
        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public ObjectPoolManager.ObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase, new(); 
        
        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <param name="allowMultiSpawn">是否允许多次获取</param>
        /// <param name="priority">优先级</param>
        /// <param name="capacity">容量</param>
        /// <param name="autoReleaseInterval">自动释放间隔</param>
        /// <param name="expireTime">过期时间</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public ObjectPoolManager.ObjectPool<T> CreateObjectPool<T>(string name, bool allowMultiSpawn,int priority , int capacity ,float autoReleaseInterval , float expireTime ) where T : ObjectBase, new();

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <param name="allowMultiSpawn">是否允许多次获取</param>
        /// <param name="priority">优先级</param>
        /// <param name="capacity">容量</param>
        /// <param name="autoReleaseInterval">自动释放间隔</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        public IObjectPool CreateObjectPool(Type objectType, string name, bool allowMultiSpawn, int priority , int capacity , float autoReleaseInterval , float expireTime );


        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        public void DestroyObjectPool<T>(string name) where T : ObjectBase, new();
        
        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        public  void DestroyObjectPool(Type objectType, string name);

        /// <summary>
        /// 是否存在对象池
        /// </summary>
        /// <param name="name">对象池名称</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public  bool HasObjectPool<T>(string name) where T : ObjectBase, new();
        
        /// <summary>
        /// 是否存在对象池
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="name">对象池名称</param>
        /// <returns></returns>
        public bool HasObjectPool(Type objectType, string name);
    }
}