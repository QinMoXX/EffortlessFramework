using System.Collections.Generic;
using System.Linq;
using AOT.Framework;
using AOT.Framework.UI;
using UnityEngine;

namespace AOT.UI
{
    public sealed class UIGroup:IUIGroup
    {
        private readonly string m_name;
        private int m_depth;
        private bool m_pause;
        private IUIGroupHelper m_uiGroupHelper;
        /// <summary>
        /// 界面组实例
        /// </summary>
        public GameObject Handle { get; }
        private readonly LinkedList<IUIForm> m_uiForms;

        public UIGroup(string mName,GameObject handle, IUIGroupHelper uiGroupHelper)
        {
            m_name = mName;
            m_depth = 0;
            m_pause = false;
            m_uiForms = new LinkedList<IUIForm>();
            Handle = handle;
            m_uiGroupHelper = uiGroupHelper;
        }

        public string Name { get => m_name; }

        public int Depth
        {
            get
            {
                return m_depth;
            }
            set
            {
                if (value == m_depth)
                {
                    return;
                }
                m_depth = value;
                m_uiGroupHelper?.SetDepth(m_name,value);
            }
        }

        public bool Pause
        {
            set
            {
                m_pause = value;
            }
            get
            {
                return m_pause;
            }
        }

        public int UIFormCount { get => m_uiForms.Count; }
        
        public bool HasUIForm(int uiFormId)
        {
            foreach (var uiForm in m_uiForms)
            {
                if (uiForm.Id == uiFormId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasUIForm(string uiFormName)
        {
            if (string.IsNullOrEmpty(uiFormName))
            {
                throw new GameFrameworkException(Utility.String.Format("This UIFormName is invalid:{0}", uiFormName));
            }
            foreach (var uiForm in m_uiForms)
            {
                if (uiForm.Name.Equals(uiFormName))
                {
                    return true;
                }
            }
            return false;
        }

        public IUIForm GetUIForm(int uiFormId)
        {
            foreach (var uiForm in m_uiForms)
            {
                if (uiForm.Id == uiFormId)
                {
                    return uiForm;
                }
            }
            return null;
        }

        public void AddUIForm(IUIForm uiForm)
        {
            IUIForm lastUIForm = null;
            foreach (var curUIForm in m_uiForms)
            {
                if (curUIForm.Depth > uiForm.Depth)
                {
                    lastUIForm = curUIForm;
                    break;
                }
            }
            if (lastUIForm != null)
            {
                var current = m_uiForms.FindLast(lastUIForm);
                m_uiForms.AddBefore(current, uiForm);
            }
            else
            {
                m_uiForms.AddLast(uiForm);
            }
        }

        public void RemoveUIForm(int uiFormId)
        {
            IUIForm lastUIForm = GetUIForm(uiFormId);
            if (lastUIForm != null)
            {
                m_uiForms.Remove(lastUIForm);
            }
        }

        public void RemoveUIForm(string uiFormName)
        {
            IUIForm lastUIForm = GetUIForm(uiFormName);
            if (lastUIForm != null)
            {
                m_uiForms.Remove(lastUIForm);
            }
        }

        public IUIForm GetUIForm(string uiFormName)
        {
            if (string.IsNullOrEmpty(uiFormName))
            {
                return null;
            }
            foreach (var uiForm in m_uiForms)
            {
                if (uiForm.Name.Equals(uiFormName))
                {
                    return uiForm;
                }
            }
            return null;
        }

        public IUIForm[] GetAllUIForms()
        {
            if (m_uiForms != null && m_uiForms.Count > 0)
            {
                return m_uiForms.ToArray();
            }
            return null;
        }
        
        public void OnUpdate(float elapaseSeconds, float realElapseSeconds)
        {
            foreach (var uiForm in m_uiForms)
            {
                if (!uiForm.Pause)
                {
                    uiForm.OnUpdate(elapaseSeconds,realElapseSeconds);
                }
            }
        }

        public int CompareTo(IUIGroup other)
        {
            return Depth - other.Depth;
        }
    }
}