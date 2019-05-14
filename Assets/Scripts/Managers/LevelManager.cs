using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class LevelManager : MonoBehaviour
    {
        private AdvertManager advertManager;

        void Start()
        {
            advertManager = GameObject.FindObjectOfType<AdvertManager>();
        }

        public void ReloadScene()
        {
            StartCoroutine(Reload());
        }

        IEnumerator Reload()
        {
            yield return new WaitForSeconds(4f);

            advertManager.advertCounter++;

            if (advertManager.advertCounter >= 4)
            {
                advertManager.ShowAdvert();
                advertManager.advertCounter = 0;
            }


            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void StopSound(string soundName)
        {
            AudioManager.instance.Pause(soundName);
        }

		public void MakePortrait()
		{
			Screen.orientation = ScreenOrientation.Portrait;
		}

		public void MakeLandscape()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
    }
}
