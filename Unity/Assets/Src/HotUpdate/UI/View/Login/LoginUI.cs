using System;
using AOT.Framework;
using AOT.Framework.Audio;
using AOT.Framework.Mvc;
using AOT.UI;
using AOT.UI.AOT.UI;
using HotUpdate.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotUpdate.UI
{
    public sealed partial class LoginUI:UIForm
    {
        public override int Id => 1;
        public override string Name => "LoginUI";
        public override string UIGroup => "Panel";
        
        private Button btn_start;
        private Button btn_setting;
        private Button btn_exit;

        private TMP_InputField  ip_username;
        private TMP_InputField ip_password;
        public override void OnInit()
        {
            btn_start = this.GetControl<Button>("btn_start");
            btn_setting = this.GetControl<Button>("btn_setting");
            btn_exit = this.GetControl<Button>("btn_exit");
            btn_start.onClick.AddListener(OnClickStart);
            btn_setting.onClick.AddListener(OnClickSetting);
            ip_username = this.GetControl<TMP_InputField>("ip_username");
            ip_password = this.GetControl<TMP_InputField>("ip_password");
        }

        private void OnClickSetting()
        {
            GameEntry.GetModule<AudioManager>().PlayAudio("DM-CGS-01","UI");
            
        }

        private Guid audioId;
        private bool isPlaying;
        private void OnClickStart()
        {
            GameEntry.GetModule<AudioManager>().PlayAudio("DM-CGS-01","UI");
            this.GetController<LoginController>().Login(ip_username.text, ip_password.text);
        }


        public override void OnShow(object param)
        {
        }

        public override void OnUpdate(float elapaseSeconds, float realElapseSeconds)
        {
        }
    }

}