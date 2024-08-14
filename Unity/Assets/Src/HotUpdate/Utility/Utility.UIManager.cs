using AOT.Framework;
using AOT.Framework.UI;
using Cysharp.Threading.Tasks;
using HotUpdate.UI.View;

    public static class UIUtility
    {
            
        public static async UniTask ShowAndTryCreateUIForm<TUIForm>(UIID uiFormId, object param = null)
            where TUIForm : class, IUIForm, new()
        {
            
            await GameEntry.GetModule<Framework.UI.UIManager>().ShowAndTryCreateUIForm<TUIForm>((int)uiFormId, param);
                
        }
        
        public static void CloseUIForm(UIID uiFormId)
        {
            
            GameEntry.GetModule<Framework.UI.UIManager>().CloseUIForm((int)uiFormId);
                
        }
    }
