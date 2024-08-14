using AOT.Framework;
using AOT.Framework.Mvc;
using AOT.UI;
using AOT.UI.AOT.UI;
using Framework.UI;
using HotUpdate.Controller;
using HotUpdate.Model;
using HotUpdate.UI.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotUpdate.UI
{
    public sealed class MenuUI:UIForm
    {
        public override int Id => 3;
        public override string Name => "MenuUI";
        public override string UIGroup => "Panel";

        private Transform panel_match;
        private Button btn_matches;
        private TextMeshProUGUI text_btn_matches;

        private TextMeshProUGUI text_username;
        private TextMeshProUGUI text_uid;
        private Button btn_setting;
        private Button btn_exit;

        private Transform text_loading;
        public override void OnInit()
        {
            panel_match = this.GetControl<Transform>("Panel/panel_match");
            btn_exit = this.GetControl<Button>("Panel/img_top/btn_exit");
            btn_setting = this.GetControl<Button>("Panel/img_top/btn_setting");
            text_username = this.GetControl<TextMeshProUGUI>("Panel/img_top/img_head/text_username");
            text_uid = this.GetControl<TextMeshProUGUI>("Panel/img_top/img_head/text_uid");
            text_loading = this.GetControl<Transform>("Panel/panel_match/text_loading");
            btn_matches = this.GetControl<Button>("Panel/panel_match/btn_matches");
            text_btn_matches = this.GetControl<TextMeshProUGUI>("Panel/panel_match/btn_matches/text");
        }

        public override void OnShow(object param)
        {
            btn_setting.onClick.AddListener(OnClickBtnSetting);
            btn_exit.onClick.AddListener(OnClickBtnExit);
            btn_matches.onClick.AddListener(OnClickMatch);

            var model = this.GetModel<LoginModel>();
            text_username.text = model.userName;
            text_uid.text = $"UIID:{model.userId.ToString()}";
        }

        private bool isMatching = false;
        private void OnClickMatch()
        {
            if (!isMatching)
            {
                this.GetController<MenuController>().ReqEntryRoom();
                text_loading.gameObject.SetActive(true);
                text_btn_matches.text = "取消匹配";
            }
            else
            {
                text_loading.gameObject.SetActive(false);  
                text_btn_matches.text = "匹配";
            }
            
            isMatching = true;

        }

        private void OnClickBtnExit()
        {
            
        }

        private void OnClickBtnSetting()
        {
            UIUtility.ShowAndTryCreateUIForm<SettingUI>(UIID.SettingUI);
        }

        public override void OnUpdate(float elapaseSeconds, float realElapseSeconds)
        {
           
        }
    }
}