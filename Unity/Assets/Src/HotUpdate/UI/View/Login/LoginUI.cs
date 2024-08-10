using System;
using AOT.Framework;
using AOT.Framework.Audio;
using AOT.UI;
using AOT.UI.AOT.UI;
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
        private  Button btn_setting;
        private Button btn_exit;
        public override void OnInit()
        {
            btn_start = this.GetControl<Button>("btn_start");
            btn_setting = this.GetControl<Button>("btn_setting");
            btn_exit = this.GetControl<Button>("btn_exit");
            btn_start.onClick.AddListener(OnClickStart);
            btn_setting.onClick.AddListener(OnClickSetting);
        }

        private void OnClickSetting()
        {
            GameEntry.GetModule<AudioManager>().PlayAudio("Bullet Impact 21","UI");
            GameEntry.GetModule<AudioManager>().PlayAudio("Railgun - Shot 6","Sound");
        }

        private Guid audioId;
        private bool isPlaying;
        private void OnClickStart()
        {
            if (isPlaying)
            {
                GameEntry.GetModule<AudioManager>().StopAudio(audioId,3);
            }
            else
            {
                IAudioHandle audioObject = GameEntry.GetModule<AudioManager>().PlayAudio("Thunder strikes 30 second- Loop", "Background", 3);
                audioId = audioObject.Guid;
                (audioObject.Target as GameObject).transform.SetParent((this.Handel as GameObject).transform);
            }

            isPlaying = !isPlaying;
        }


        public override void OnShow(object param)
        {
        }

        public override void OnUpdate(float elapaseSeconds, float realElapseSeconds)
        {
        }
    }

}