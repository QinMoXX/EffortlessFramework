using System;

namespace AOT.Framework.UI
{
    /// <summary>
    /// 界面组接口
    /// </summary>
    public interface IUIGroup:IComparable<IUIGroup>
    {
        /// <summary>
        /// 获取界面组名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 界面组深度
        /// </summary>
        int Depth { get; set; }
        
        /// <summary>
        /// 界面组是否暂停
        /// </summary>
        bool Pause { get; set; }
        
        /// <summary>
        /// 获取界面中界面数量
        /// </summary>
        int UIFormCount { get; }

        /// <summary>
        /// 界面是否存在
        /// </summary>
        /// <param name="uiFormId">UIID</param>
        /// <returns></returns>
        bool HasUIForm(int uiFormId);

        /// <summary>
        /// 界面是否存在
        /// </summary>
        /// <param name="uiFormName"></param>
        /// <returns></returns>
        bool HasUIForm(string uiFormName);

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormId"></param>
        /// <returns></returns>
        IUIForm GetUIForm(int uiFormId);

        /// <summary>
        /// 添加界面
        /// </summary>
        /// <param name="uiForm">界面</param>
        void AddUIForm(IUIForm uiForm);

        /// <summary>
        /// 移除界面
        /// </summary>
        /// <param name="uiFormId"></param>
        void RemoveUIForm(int uiFormId);

        /// <summary>
        /// 移除界面
        /// </summary>
        /// <param name="uiFormName"></param>
        void RemoveUIForm(string uiFormName);

        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormName">界面名称</param>
        /// <returns></returns>
        IUIForm GetUIForm(string uiFormName);

        /// <summary>
        /// 从界面组中获取界面
        /// </summary>
        /// <returns></returns>
        IUIForm[] GetAllUIForms();

        /// <summary>
        /// 界面组轮询
        /// </summary>
        /// <param name="elapaseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        void OnUpdate(float elapaseSeconds, float realElapseSeconds);
    }
}