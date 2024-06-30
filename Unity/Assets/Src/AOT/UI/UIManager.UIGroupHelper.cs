using System;
using AOT.Environment;
using AOT.Framework;
using AOT.Framework.UI;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AOT.UI
{
    public sealed class UIGroupHelper:IUIGroupHelper
    {
        public const string UI_GROUP_PATH = "UIManager";

        private GameObject root;
        private int m_depth;

        public UIGroupHelper()
        {
            root = GameObject.Find(UI_GROUP_PATH);
            if (root == null)
            {
                CreateCanvas();
            }
        }
        
        /// <summary>
        /// 创建界面组
        /// </summary>
        /// <param name="uiGroupName"></param>
        /// <returns></returns>
        public IUIGroup Create(string uiGroupName)
        {
            if (root == null)
            {
                CreateCanvas();
            }
            GameObject groupGameObject = new GameObject(uiGroupName,typeof(RectTransform),typeof(CanvasGroup));
            groupGameObject.layer = 5;
            groupGameObject.transform.SetParent(root.transform, false);
            groupGameObject.SetActive(true);
            RectTransform rectTransform = groupGameObject.GetComponent<RectTransform>();
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = Vector2.zero;
            return new UIGroup(uiGroupName,groupGameObject,this);
            
        }

        private void CreateCanvas()
        {
            root = new GameObject(UI_GROUP_PATH,typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            root.layer = 5;
            Canvas canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = EnvCamera.UICamera;
            root.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(root);
        }

        public void SetDepth(string uiGroupName,int depth)
        {
            if (root == null)
            {
                CreateCanvas();
            }
            IUIGroup[] curGroups = GameEntry.GetModule<UIManager>().GetAllUIGroup();
            Array.Sort<IUIGroup>(curGroups);
            for (int i = 0,j = curGroups.Length; i < j; i++)
            {
                (curGroups[i] as UIGroup)?.Handle.transform.SetSiblingIndex(i);
            }
        }
    }
}