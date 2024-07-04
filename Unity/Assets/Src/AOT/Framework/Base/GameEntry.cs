using System;

namespace AOT.Framework
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public static class GameEntry
    {
        private static readonly SortList<Int16, IGameModule> _GameModule = new SortList<short, IGameModule>();
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var keyValue in _GameModule.List)
            {
                keyValue.Item2.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 退出所有模块
        /// </summary>
        public static void Exit()
        {
            foreach (var keyValue in _GameModule.List)
            {
                keyValue.Item2?.Destroy();
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
            foreach (var keyValue in _GameModule.List)
            {
                if (keyValue.Item2.GetType() == typeof(T))
                {
                    return keyValue.Item2 as T;
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
            foreach (var keyValue in _GameModule.List)
            {
                if (keyValue.Item2.GetType() == moduleType)
                {
                    return keyValue.Item2;
                }
            }

            return null;
        }
        

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>返回已创建模块</returns>
        public static T CreatModule<T>() where T:class,IGameModule, new()
        {
#if UNITY_EDITOR
            //编辑器下进行模块检查，实际运行提高性能可不进行模块检查
            ModuleInspect<T>();
#endif
            
            IGameModule module = new T();
            _GameModule.Add( module.Priority,module);
            module.Init();
            return module as T;
        }

        /// <summary>
        /// 模块检擦
        /// </summary>
        /// <typeparam name="T">被检查模块类型</typeparam>
        /// <returns>是否通过检查</returns>
        /// <exception cref="GameFrameworkException"></exception>
        private static void ModuleInspect<T>() where T:class,IGameModule, new()
        {
            Type moduleType = typeof(T);
            //获取DependencyModule特性
            var dependencyAttributes = moduleType.GetCustomAttributes(typeof(DependencyModuleAttribute), false);
            //存在特性判断
            if (dependencyAttributes.Length > 0)
            {
                //获取依赖模块
                var dependencyModules = dependencyAttributes[0] as DependencyModuleAttribute;
                //遍历依赖模块
                foreach (var dependencyModule in dependencyModules.Dependencies)
                {
                    //判断依赖模块是否存在
                    if (!HasModule(dependencyModule))
                    {
                        throw new GameFrameworkException((Utility.String.Format("Dependency module {0} is not exist, please create before {1} module.",
                            dependencyModule.Name, moduleType.FullName)));
                    }
                }
            }
        }

        /// <summary>
        /// 判断模块是否存在
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns>是否存在</returns>
        private static bool HasModule(Type moduleType)
        {
            foreach (var keyValue in _GameModule.List)
            {
                if (keyValue.Item2.GetType() == moduleType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}