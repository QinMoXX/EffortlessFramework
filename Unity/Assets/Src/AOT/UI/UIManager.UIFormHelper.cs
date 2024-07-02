using System;
using AOT.Framework.Resource;
using AOT.Framework.UI;
using Cysharp.Threading.Tasks;
using Framework.UI;
using UnityEngine;

namespace AOT.UI
{
    public sealed class UIFormHelper:IUIFormHelper
    {
        private UIManager m_uiManager;
        private ResourceManager m_resourceManager;
        
        public  UIFormHelper(UIManager uiManager, ResourceManager resourceManager)
        {
            m_uiManager = uiManager;
            m_resourceManager = resourceManager;
        }
        /// <summary>
        /// 异步创建方法
        /// </summary>
        /// <param name="uiFormType"></param>
        /// <returns></returns>
        public async UniTask<IUIForm> Create(Type uiFormType)
        {
            //创建界面类
            UIForm uiForm = (UIForm)Activator.CreateInstance(uiFormType);
            GameObject gameObject = await m_resourceManager.LoadAssetAsync<GameObject>(uiForm.Name);
            UIGroup uiGroup = m_uiManager.GetUIGroup(uiForm.UIGroup) as UIGroup;
            GameObject uiGo = GameObject.Instantiate(gameObject,uiGroup.Handle.transform,false);
            uiGo.SetActive(false);
            uiForm.Pause = true;
            uiForm.Handel = uiGo;
            uiForm.Init();
            uiGroup.AddUIForm(uiForm);
            return uiForm;
        }

        public void ShowUIForm(IUIForm uiForm)
        {
            GameObject uiGameObject = uiForm.Handel as GameObject;
            uiGameObject.SetActive(true);
            //开启交互
            uiGameObject.GetComponent<CanvasGroup>().interactable = true;
            uiForm.Pause = false;
        }

        public void CloseUIForm(IUIForm uiForm)
        {
            uiForm.Pause = true;
            GameObject uiGameObject = uiForm.Handel as GameObject;
            uiGameObject.SetActive(false);
            //禁用交互
            uiGameObject.GetComponent<CanvasGroup>().interactable = false;
        }

        public void ReleaseUIForm(IUIForm uiForm)
        {
            GameObject.Destroy(uiForm.Handel as GameObject);
        }


        
        

    }
}