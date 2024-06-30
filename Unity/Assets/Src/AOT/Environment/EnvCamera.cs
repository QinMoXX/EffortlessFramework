using UnityEngine;

namespace AOT.Environment
{
    public class EnvCamera
    {
        private static Camera m_mainCamera;
        /// <summary>
        /// 场景主相机
        /// </summary>
        public static Camera MainCamera
        {
            get
            {
                if (m_mainCamera == null)
                {
                    m_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                }
                return m_mainCamera;
            }
        }

        private static Camera m_uiCamera;

        /// <summary>
        /// 界面相机
        /// </summary>
        public static Camera UICamera
        {
            get
            {
                if (m_uiCamera == null)
                {
                    m_uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
                }
                return m_uiCamera;
            }
        }
    }
}