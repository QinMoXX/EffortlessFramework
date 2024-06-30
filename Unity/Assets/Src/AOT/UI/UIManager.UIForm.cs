using System.Threading;
using AOT.Framework.Mvc;
using AOT.Framework.UI;

namespace AOT.UI
{
    public abstract class UIForm:IUIForm,IView
    {
        public CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string UIGroup { get; }
        public int Depth { get; set; }
        private bool m_pause;
        public bool Pause { get; set; }
        public object Handel { get ; set; }
        
        public void Init()
        {
            OnInit();
        }

        public abstract void OnInit();
        public abstract void OnShow(object param);

        public virtual void OnClose()
        {
            //界面关闭的时候取消UniTask
            cancellationToken?.Cancel();
        }

        public virtual void OnResume()
        {

        }
        public abstract void OnUpdate(float elapaseSeconds, float realElapseSeconds);

        public virtual void OnDepthChanged(int depthInUIGroup)
        {
            
        }


    }
}