using System;
using System.Runtime.CompilerServices;
using DataStructure;
using GameFramework;

namespace Framework
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public static class GameEntry
    {
        private static readonly PriorityQueue<IGameModule, uint> _GameModule = new PriorityQueue<IGameModule, uint>();
        
        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (IGameModule module in _GameModule.List)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 退出所有模块
        /// </summary>
        public static void Exit()
        {
            foreach (IGameModule module in _GameModule.List)
            {
                module.Exit();
            }
            _GameModule.Clear();
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : class, IGameModule
        {
            foreach (IGameModule module  in _GameModule.List)
            {
                if (module.GetType() == typeof(T))
                {
                    return module as T;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns></returns>
        public static IGameModule GetModule(Type moduleType)
        {
            foreach (IGameModule module in _GameModule.List)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return null;
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        /// <exception cref="GameFrameworkException"></exception>
        public static IGameModule CreatModule(Type moduleType)
        {
            IGameModule module = Activator.CreateInstance(moduleType) as IGameModule;
            if (module == null)
            {
                throw new GameFrameworkException(Utility.String.Format("Can not create module '{0}'.",moduleType.FullName));
            }
            _GameModule.Enqueue(module, module.Priority);
            module.Init();
            return module;
        }
    }
}