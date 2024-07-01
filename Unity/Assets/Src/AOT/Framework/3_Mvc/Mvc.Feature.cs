using AOT.Framework.Event;

namespace AOT.Framework.Mvc
{
    public static class MvcExtension
    {
        /// <summary>
        /// 发送事件，包含全部组。线程不安全，立即执行
        /// </summary>
        /// <param name="self">事件发送者</param>
        /// <param name="sender">事件发送对象</param>
        /// <param name="e">事件传递数据</param>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        /// <typeparam name="TGroup">事件组类型</typeparam>
        public static void SendNow<TEventArgs, TGroup>(this IEventSender self,object sender,TEventArgs e) 
            where TEventArgs : struct where TGroup : EventBasis
        {
            EventManager.Instance.SendNow<TEventArgs,TGroup>(sender,e);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="self">事件接收者</param>
        /// <param name="onEvent">事件方法</param>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        /// <typeparam name="TGroup">事件组类型</typeparam>
        public static void Register<TEventArgs, TGroup>(this IEventReceiver self, UltraEventHandler<TEventArgs> onEvent)
            where TEventArgs : struct where TGroup : EventBasis
        {
            EventManager.Instance.Register<TEventArgs,TGroup>(onEvent);
        }
        
        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="self">事件接收者</param>
        /// <param name="onEvent">事件方法</param>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        /// <typeparam name="TGroup">事件组</typeparam>
        public static void UnRegister<TEventArgs, TGroup>(this IEventReceiver self, UltraEventHandler<TEventArgs> onEvent)
            where TEventArgs : struct where TGroup : EventBasis
        {
            EventManager.Instance.UnRegister<TEventArgs,TGroup>(onEvent);
        }

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <param name="self">控制器获取者</param>
        /// <typeparam name="TController">控制器类型</typeparam>
        /// <returns></returns>
        public static TController GetController<TController>(this IControllerGetter self)
            where TController : class, IController, new()
        {
            return MvcManager.Instance.GetController<TController>() as TController;
        }

        /// <summary>
        /// 获取数据模型
        /// </summary>
        /// <param name="self">模型获取者</param>
        /// <typeparam name="TModel">数据模型类型</typeparam>
        /// <returns></returns>
        public static TModel GetModel<TModel>(this IModelGetter self) where TModel : class, IModel, new()
        {
            return MvcManager.Instance.GetModel<TModel>() as TModel;
        }
    }
}