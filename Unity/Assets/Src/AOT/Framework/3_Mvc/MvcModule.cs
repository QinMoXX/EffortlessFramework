using System;
using System.Collections.Generic;
using Framework;

namespace Src.AOT.Framework.Mvc
{
    /// <summary>
    /// MVC 模块
    /// </summary>
    public class MvcModule:IGameModule
    {
        public short Priority => 3;

        private Dictionary<Type, IModel> m_ModelDic; //数据类集合
        private Dictionary<Type, IView> m_ViewDic; //视图类集合
        private Dictionary<Type, IController> m_ControllerDic; //控制器集合

        public void Init()
        {
            m_ModelDic = new Dictionary<Type, IModel>();
            m_ViewDic = new Dictionary<Type, IView>();
            m_ControllerDic = new Dictionary<Type, IController>();
        }

        public void Update(float virtualElapse, float realElapse)
        {
            
        }

        public void Destroy()
        {
            m_ModelDic.Clear();
            m_ViewDic.Clear();
            m_ControllerDic.Clear();
        }

        /// <summary>
        /// 获取视图类
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <returns>视图类</returns>
        public IView GetView<T>() where T : class, IView,new()
        {
            Type viewType = typeof(T);
            IView view;
            if (!m_ViewDic.TryGetValue(viewType,out view))
            {
                view = new T();
                view.Init();
                m_ViewDic.Add(viewType,view);
            }
            return view;
        }

        /// <summary>
        /// 获取视图类(该方法有性能开销推荐使用GetView的泛型方法)
        /// </summary>
        /// <param name="viewType">视图类型</param>
        /// <returns>视图类</returns>
        public IView GetView(Type viewType)
        {
            IView view;
            if (!m_ViewDic.TryGetValue(viewType, out view))
            {
                view = Activator.CreateInstance(viewType) as IView;
                view?.Init();
                m_ViewDic.Add(viewType, view);
            }
            return view;
        }

        /// <summary>
        /// 获取数据模型类
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <returns>数据模型类</returns>
        public IModel GetModel<T>() where T : class, IModel, new()
        {
            Type modelType = typeof(T);
            IModel model;
            if (!m_ModelDic.TryGetValue(modelType, out model))
            {
                model = new T();
                model.Init();
                m_ModelDic.Add(modelType, model);
            }

            return model;
        }
        
        /// <summary>
        /// 获取数据模型类(该方法有性能开销推荐使用GetModel的泛型方法)
        /// </summary>
        /// <param name="modelType">数据模型类型</param>
        /// <returns>数据模型类</returns>
        public IModel GetModel(Type modelType)
        {
            IModel model;
            if (!m_ModelDic.TryGetValue(modelType, out model))
            {
                model = Activator.CreateInstance(modelType) as IModel;
                model?.Init();
                m_ModelDic.Add(modelType, model);
            }
            return model;
        }

        /// <summary>
        /// 获取控制器类
        /// </summary>
        /// <typeparam name="T">控制器类型</typeparam>
        /// <returns>控制器类</returns>
        public IController GetController<T>() where T : class, IController, new()
        {
            Type controllerType = typeof(T);
            IController controller;
            if (!m_ControllerDic.TryGetValue(controllerType, out controller))
            {
                controller = new T();
                controller.Init();
                m_ControllerDic.Add(controllerType, controller);
            }
            return controller;
        }
        
        /// <summary>
        /// 获取控制器类(该方法有性能开销推荐使用GetController的泛型方法)
        /// </summary>
        /// <param name="controllerType">控制器类型</param>
        /// <returns>控制器类</returns>
        public IController GetController(Type controllerType)
        {
            IController controller;
            if (!m_ControllerDic.TryGetValue(controllerType, out controller))
            {
                controller = Activator.CreateInstance(controllerType) as IController;
                controller?.Init();
                m_ControllerDic.Add(controllerType, controller);
            }
            return controller;
        }
    }
}