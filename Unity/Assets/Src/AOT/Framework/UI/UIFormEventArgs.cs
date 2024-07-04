using AOT.Framework.UI;

namespace AOT.Framework.UI
{
    /// <summary>
    /// 打开界面事件。
    /// </summary>
    public class OpenUIFormEventArgs :IReference
    {
        /// <summary>
        /// 获取打开的界面
        /// </summary>
        public IUIForm UIForm { get; private set; }
        
        /// <summary>
        /// 获取用户自定义数据
        /// </summary>
        public object UserData { get; private set; }
        
        /// <summary>
        /// 获取界面打开状态
        /// </summary>
        public bool OpenSuccess { get; private set; }
        
        public OpenUIFormEventArgs()
        {
            UIForm = null;
            UserData = null;
            OpenSuccess = false;
        }
        
        public void Clear()
        {
            UIForm = null;
            UserData = null;
            OpenSuccess = false;
        }
    }

    public class CloseUIFormEventArgs : IReference
    {
        /// <summary>
        /// 获取用户界面
        /// </summary>
        public IUIForm UIForm { get; private set; }

        public CloseUIFormEventArgs()
        {
            UIForm = null;
        }
        
        public void Clear()
        {
            UIForm = null;
        }
    }
    
    
}