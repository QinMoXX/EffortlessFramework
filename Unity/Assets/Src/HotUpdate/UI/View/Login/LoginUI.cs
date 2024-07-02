using AOT.UI;
using UnityEngine;

namespace HotUpdate.UI
{
    public sealed class LoginUI:UIForm
    {
        public override int Id => 1;
        public override string Name => "LoginUI";
        public override string UIGroup => "Panel";
        public override void OnInit()
        {
            
        }

        public override void OnShow(object param)
        {
            Debug.Log("LoginUI OnShow");
            Debug.Log(TableManager.Instance.CfgClientTable.TbItem.GetOrDefault(10004).Name);
        }

        public override void OnUpdate(float elapaseSeconds, float realElapseSeconds)
        {
            
        }
    }

}