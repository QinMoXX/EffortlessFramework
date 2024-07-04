using System;
using Cysharp.Threading.Tasks;

namespace AOT.Framework.UI
{
    /// <summary>
    /// 界面辅助接口
    /// </summary>
    public interface IUIFormHelper
    {
        UniTask<IUIForm> Create(Type uiFormType);

        void ShowUIForm(IUIForm uiForm);

        void CloseUIForm(IUIForm uiForm);

        void ReleaseUIForm(IUIForm uiForm);
    }
}