using System;
using SimpleJSON;

namespace AOT.Framework.Persistent
{
    /// <summary>
    /// 配置管理器，负责配置的加载、保存和访问。
    /// </summary>
    public sealed class ConfigManager: SingletonInstance<IConfigManager>, IGameModule, IConfigManager
    {
        public short Priority => 3;
        private  IConfigHelper m_ConfigHelper;
        /// <summary>
        /// 初始化配置管理器。
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 使用配置助手初始化配置管理器。
        /// </summary>
        /// <param name="configHelper">配置助手实例。</param>
        public IConfigManager Initialize(IConfigHelper configHelper)
        {
            m_ConfigHelper = configHelper;
            return this;
        }

        /// <summary>
        /// 每帧更新配置管理器。
        /// </summary>
        /// <param name="virtualElapse">虚拟时间流逝。</param>
        /// <param name="realElapse">实际时间流逝。</param>
        public void Update(float virtualElapse, float realElapse)
        {
        }

        /// <summary>
        /// 销毁配置管理器。
        /// </summary>
        public void Destroy()
        {
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        public void Load()
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.Load();
        }

        /// <summary>
        /// 保存配置。
        /// </summary>
        public void Save()
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.Save();
        }

        /// <summary>
        /// 检查是否有特定配置。
        /// </summary>
        /// <returns>如果存在返回true，否则返回false。</returns>
        public bool HasConfig()
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            return m_ConfigHelper.HasConfig();
        }

        /// <summary>
        /// 清除配置。
        /// </summary>
        public void Clear()
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.Clear();
        }

        /// <summary>
        /// 设置整型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="value">配置值。</param>
        public void SetInt(string configKey, int value)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.SetInt(configKey, value);
        }

        /// <summary>
        /// 设置浮点型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="value">配置值。</param>
        public void SetFloat(string configKey, float value)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.SetFloat(configKey, value);
        }

        /// <summary>
        /// 设置字符串型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="value">配置值。</param>
        public void SetString(string configKey, string value)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.SetString(configKey, value);
        }

        /// <summary>
        /// 设置布尔型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="value">配置值。</param>
        public void SetBool(string configKey, bool value)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.SetBool(configKey, value);
        }

        /// <summary>
        /// 获取整型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>配置值。</returns>
        public int GetInt(string configKey, int defaultValue)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            return m_ConfigHelper.GetInt(configKey, defaultValue);
        }

        /// <summary>
        /// 获取浮点型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>配置值。</returns>
        public float GetFloat(string configKey, float defaultValue)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            return m_ConfigHelper.GetFloat(configKey, defaultValue);
        }

        /// <summary>
        /// 获取字符串型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>配置值。</returns>
        public string GetString(string configKey, string defaultValue)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            return m_ConfigHelper.GetString(configKey, defaultValue);
        }

        /// <summary>
        /// 获取布尔型配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>配置值。</returns>
        public bool GetBool(string configKey, bool defaultValue)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            return m_ConfigHelper.GetBool(configKey, defaultValue);
        }

        /// <summary>
        /// 设置JSON配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <param name="value">配置值。</param>
        public void SetJson(string configKey, JSONNode value)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            m_ConfigHelper.SetJson(configKey, value);
        }

        /// <summary>
        /// 获取JSON配置。
        /// </summary>
        /// <param name="configKey">配置键。</param>
        /// <returns>配置值。</returns>
        public JSONNode GetJson(string configKey)
        {
            if (m_ConfigHelper == null)
            {
                throw new GameFrameworkException("ConfigHelper does not exist");
            }
            return m_ConfigHelper.GetJson(configKey);
        }
    }
}
