using AOT.Framework;
using Framework.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AOT.UI
{
    namespace AOT.UI
    {
        public static class UIFormExtension
        {
            /// <summary>
            /// 获取界面中组件
            /// </summary>
            /// <param name="self">界面<see cref="UIForm"/></param>
            /// <param name="path">路径字符串</param>
            /// <typeparam name="T">UGUI组件类型<see cref="UIBehaviour"/>></typeparam>
            /// <returns></returns>
            /// <exception cref="UIException"></exception>
            public static T GetControl<T>(this UIForm self,string path)where T:UIBehaviour{
                if (self.Handel == null)
                {
                    throw new GameFrameworkException("UIForm intstance object does not exist❗");
                }

                Transform chiledTransform = (self.Handel as GameObject).transform.Find(path);
                if (chiledTransform == null)
                {
                    throw new GameFrameworkException("UIForm Control is exist❗");
                }
                return chiledTransform.GetComponent<T>();
            }

            /// <summary>
            /// 关闭界面关闭自身方法
            /// </summary>
            /// <param name="self">界面<see cref="UIForm"/>></param>
            public static void CloseSelfFunc(this UIForm self)
            {
                GameEntry.GetModule<UIManager>()?.CloseUIForm(self.Id);
            }
        }
    }
}