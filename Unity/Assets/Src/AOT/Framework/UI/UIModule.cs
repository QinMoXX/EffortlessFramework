using System;
using System.Collections.Generic;
using System.Linq;
using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Mvc;
using AOT.Framework.UI;
using Cysharp.Threading.Tasks;

namespace Framework.UI
{
    [DependencyModule(typeof(MvcManager))]
    public partial class UIManager:IGameModule
    {
        public short Priority => 10;

        #region 数据

        private Dictionary<string, IUIGroup> m_uiGroupDic;
        private Dictionary<string, int> m_uiFormNameDic;
        private Dictionary<int, IUIForm> m_uiFormDic;

        private IUIGroupHelper m_uiGroupHelper;
        /// <summary>
        /// 界面组辅助器
        /// </summary>
        /// <exception cref="UIException"></exception>
        public IUIGroupHelper UIGroupHelper
        {
            get
            {
                if (m_uiGroupHelper == null)
                {
                    throw new GameFrameworkException("Missing UIGroupHelper, please initialize UIManager correctly!");
                }
                return m_uiGroupHelper;
            }
        }
        
        private IUIFormHelper m_uiFormHelper;
        /// <summary>
        /// 界面辅助器
        /// </summary>
        /// <exception cref="UIException"></exception>
        public IUIFormHelper UIFormHelper
        {
            get
            {
                if (m_uiFormHelper == null)
                {
                    throw new GameFrameworkException("Missing UIFormHelper, please initialize UIManager correctly!");
                }
                return m_uiFormHelper;
            }
        }
        
        #endregion

        #region Event
        public event UltraEventHandler<OpenUIFormEventArgs> OnOpenUIFormSuccess;
        public event UltraEventHandler<CloseUIFormEventArgs> OnCloseUIFormSuccess;
        #endregion

        public void Initialize(IUIGroupHelper uiGroupHelper, IUIFormHelper uiFormHelper)
        {
            m_uiGroupHelper = uiGroupHelper;
            m_uiFormHelper = uiFormHelper;
        }
        
        public void Init()
        {
            m_uiGroupDic = new Dictionary<string, IUIGroup>(4);
            m_uiFormNameDic = new Dictionary<string, int>(16);
            m_uiFormDic = new Dictionary<int, IUIForm>(16);
        }

        public void Update(float virtualElapse, float realElapse)
        {
            
        }
        
        /// <summary>
        /// 获取UI组
        /// </summary>
        /// <param name="uiGroupName">ui组名</param>
        /// <returns></returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException($"This UIGroupName is invalid： {uiGroupName}");
            }
            if (m_uiGroupDic.TryGetValue(uiGroupName, out var uiGroup))
            {
                return uiGroup;
            }
            return null;
        }
        
        /// <summary>
        /// 获取界面组(有性能损耗推荐使用 GetAllUIGroupCollection 方法)
        /// </summary>
        /// <returns>返回数组</returns>
        public IUIGroup[] GetAllUIGroup()
        {
            return m_uiGroupDic.Values.ToArray();
        }
        
        /// <summary>
        /// 获取界面组集合
        /// </summary>
        /// <returns>返回集合</returns>
        public ICollection<IUIGroup> GetAllUIGroupCollection()
        {
            return m_uiGroupDic.Values;
        }
        
        /// <summary>
        /// 添加UI组
        /// </summary>
        /// <param name="uiGroupName">UI组名称</param>
        /// <param name="uiGroupDepth">UI组深度</param>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException(Utility.String.Format("This UIGroupName is invalid： {0}",uiGroupName));
            }

            if (m_uiGroupDic.ContainsKey(uiGroupName))
            {
                EDebug.Log("This UIGroup already exists!");
                return false;
            }
            IUIGroup uiGroup = UIGroupHelper.Create(uiGroupName);
            m_uiGroupDic.TryAdd(uiGroupName, uiGroup);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(Utility.String.Format("This UIGroup Creations fails：{1} ",uiGroupName));
            }
            uiGroup.Depth = uiGroupDepth;
            EDebug.Log(Utility.String.Format("This UIGroup {0} Creations success!",uiGroupName));
            return true;
        }
        
        /// <summary>
        /// 是否存在界面
        /// </summary>
        /// <param name="uiFormId">界面id</param>
        /// <returns></returns>
        public bool HasUIForm(int uiFormId)
        {
            if (m_uiFormDic.TryGetValue(uiFormId, out var uiForm))
            {
                return uiForm != null;
            }
            return false;
        }
        
        /// <summary>
        /// 是否存在界面，不唯一存在,性能不好,推荐使用id作为查询条件<see cref="HasUIForm(int)"/>
        /// </summary>
        /// <param name="uiFormName">界面资源名</param>
        /// <returns></returns>
        public bool HasUIForm(string uiFormName)
        {
            if (string.IsNullOrEmpty(uiFormName))
            {
                return false;
            }
            foreach (var value in m_uiFormDic.Values)
            {
                if (uiFormName.Equals(value.Name))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="uiFormId">界面id</param>
        /// <returns></returns>
        public IUIForm GetUIForm(int uiFormId)
        {
            if (m_uiFormDic.TryGetValue(uiFormId, out var uiForm))
            {
                return uiForm;
            }
            return null;
        }
        
        /// <summary>
        /// 获取界面,不唯一存在,性能不好,推荐使用id作为查询条件<see cref="GetUIForm(int)"/>
        /// </summary>
        /// <param name="uiFormName">界面资源名</param>
        /// <returns></returns>
        public IUIForm GetUIForm(string uiFormName)
        {
            if (string.IsNullOrEmpty(uiFormName))
            {
                return null;
            }
            foreach (var value in m_uiFormDic.Values)
            {
                if (uiFormName.Equals(value.Name))
                {
                    return value;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 创建界面
        /// </summary>
        /// <typeparam name="TUIForm">界面类型</typeparam>
        public async UniTask<TUIForm> CreateUIForm<TUIForm>() where TUIForm : IUIForm, new()
        {
            IUIForm uiForm = await UIFormHelper.Create(typeof(TUIForm));
            m_uiFormDic.TryAdd(uiForm.Id, uiForm);
            m_uiFormNameDic.TryAdd(uiForm.Name, uiForm.Id);
            return (TUIForm)uiForm;
        }
        
        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="uiFormId">界面id</param>
        /// <param name="param">用户自定义数据</param>
        public void ShowUIForm(int uiFormId, object param = null)
        {
            if (!m_uiFormDic.TryGetValue(uiFormId, out var uiForm))
            {
                throw  new GameFrameworkException(Utility.String.Format("This UIForm is not Created：{0}",uiFormId));
            }
            UIFormHelper.ShowUIForm(uiForm);
            uiForm.OnShow(param);
        }
        
        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="uiFormName">界面资源名</param>
        /// <param name="param">用户自定义数据</param>
        public void ShowUIForm(string uiFormName, object param = null)
        {
            if (string.IsNullOrEmpty(uiFormName))
            {
                throw new GameFrameworkException($"This UIFormName is invalid： {uiFormName}");
            }
            
            if (!m_uiFormNameDic.TryGetValue(uiFormName, out int uiFormId))
            {
                throw new GameFrameworkException((Utility.String.Format("This UIForm is not Created：{0}", uiFormName)));
            }
            ShowUIForm(uiFormId, param);
        }

        /// <summary>
        /// 显示界面，如果界面不存在则创建
        /// </summary>
        /// <param name="uiFormId">界面id</param>
        /// <param name="param">用户自定义数据</param>
        /// <typeparam name="TUIForm">界面类型</typeparam>
        public async UniTask ShowAndTryCreateUIForm<TUIForm>(int uiFormId,object param = null) where TUIForm :class, IUIForm, new()
        {
            if (!m_uiFormDic.TryGetValue(uiFormId, out var uiForm))
            {
                uiForm = await CreateUIForm<TUIForm>();
            }
            UIFormHelper.ShowUIForm(uiForm);
            uiForm.OnShow(param);
        }
        
        /// <summary>
        /// 显示界面，如果界面不存在则创建
        /// </summary>
        /// <param name="uiFormName">界面资源名</param>
        /// <param name="param">用户自定义数据</param>
        /// <typeparam name="TUIForm">界面类型</typeparam>
        public async UniTask ShowAndTryCreateUIForm<TUIForm>(string uiFormName,object param = null) where TUIForm :class, IUIForm, new()
        {
            TUIForm uiForm = null;
            if (!m_uiFormNameDic.TryGetValue(uiFormName, out int uiFormId))
            {
                uiForm = await CreateUIForm<TUIForm>();
            }
            UIFormHelper.ShowUIForm(uiForm);
            uiForm.OnShow(param);
        }
        
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiFormId">界面id</param>
        /// <returns></returns>
        public bool CloseUIForm(int uiFormId)
        {
            if (!m_uiFormDic.TryGetValue(uiFormId, out var uiForm))
            {
                return false;
            }
            UIFormHelper.CloseUIForm(uiForm);
            uiForm.OnClose();
            return true;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiFormName">界面资源名</param>
        /// <returns></returns>
        public bool CloseUIForm(string uiFormName)
        {
            if (string.IsNullOrEmpty(uiFormName))
            {
                throw new GameFrameworkException(Utility.String.Format("This UIFormName is invalid： {0}", uiFormName));
            }
            
            if (!m_uiFormNameDic.TryGetValue(uiFormName, out int uiFormId))
            {
                return false;
            }
            return CloseUIForm(uiFormId);
        }
        

        public void Destroy()
        {
            OnOpenUIFormSuccess = null;
            OnCloseUIFormSuccess = null;
            m_uiGroupDic.Clear();
            m_uiGroupDic = null;
            m_uiFormDic.Clear();
            m_uiFormDic = null;
            m_uiFormNameDic.Clear();
            m_uiFormNameDic = null;
        }
    }
}