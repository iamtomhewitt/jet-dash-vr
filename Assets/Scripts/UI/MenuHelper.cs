using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Manager;

namespace UI
{
    public class MenuHelper : MonoBehaviour
    {
        public bool VRMode;
        public Image VRModeImage;
        public Slider sensitivitySlider;
        public Text VRCountdownText;
        public GameObject mainMenu;

        private string gameSceneName;

        void Start()
        {
            // Reset GameSettings values
            GameSettings.instance.isVRMode = VRMode;
            VRModeImage.enabled = VRMode;
            GameSettings.instance.sensitivity = sensitivitySlider.value;
        }

        public void SwitchMode()
        {
            VRMode = !VRMode;
            VRModeImage.enabled = !VRModeImage.enabled;
            GameSettings.instance.isVRMode = VRMode;
        }

        public void PlayUISound()
        {
            AudioManager.instance.Play("UI Select");
        }

        public void Play(string scene)
        {		
            SceneManager.LoadScene(scene);
        }

        public void SetSensitivity()
        {
            GameSettings.instance.sensitivity = sensitivitySlider.value;
        }

        public void VRHeadsetCountdown()
        {
            if (GameSettings.instance.isVRMode)
                StartCoroutine(HeadsetCountdown());
            else
                Play(gameSceneName);
        }


        public void SetPlaySceneName(string n)
        {
            gameSceneName = n;
        }

        IEnumerator HeadsetCountdown()
        {
			Screen.orientation = ScreenOrientation.LandscapeLeft;

            mainMenu.SetActive(false);
            int countdown = 10;

            for (int i = countdown; i > 0; i--)
            {
                VRCountdownText.text = "PUT ON YOUR VR HEADSET \n" + i;
                yield return new WaitForSeconds(1.5f);
            }

            VRCountdownText.text = "";

            Play(gameSceneName);
        }
    }
}
