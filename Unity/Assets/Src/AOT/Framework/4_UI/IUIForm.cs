
namespace AOT.Framework.UI
{
    /// <summary>
    /// 界面接口
    /// </summary>
    public interface IUIForm
    {
        /// <summary>
        /// 界面id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 获取界面名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 获取所属界面组
        /// </summary>
        string UIGroup { get; }
        
        /// <summary>
        /// 界面深度
        /// </summary>
        int Depth { get; set; }
        
        /// <summary>
        /// 界面暂停
        /// </summary>
        bool Pause { get; set; }
        
        /// <summary>
        /// 获取界面实例
        /// </summary>
        object Handel { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        void OnInit();

        /// <summary>
        /// 界面打开
        /// </summary>
        /// <param name="param">用户自定义数据</param>
        void OnShow(object param);

        /// <summary>
        /// 界面关闭
        /// </summary>
        void OnClose();

        /// <summary>
        /// 界面暂停恢复
        /// </summary>
        void OnResume();

        /// <summary>
        /// 界面轮询
        /// </summary>
        /// <param name="elapaseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        void OnUpdate(float elapaseSeconds, float realElapseSeconds);

        /// <summary>
        /// 界面深度改变
        /// </summary>
        /// <param name="depthInUIGroup">界面在界面组中的深度</param>
        void OnDepthChanged(int depthInUIGroup);
    }
}