namespace AOT.Framework.Mvc
{
    public interface IModel
    {
        public void Init();
    }

    public interface IView
    {
        public void Init();
    }

    public interface IController
    {
        public void Init();
    }
    
    //事件发送者
    public interface IEventSender{}
    //事件接收者
    public interface IEventReceiver{}
    //控制器获取者
    public interface  IControllerGetter{}
    //模块获取者
    public interface IModelGetter{}
}