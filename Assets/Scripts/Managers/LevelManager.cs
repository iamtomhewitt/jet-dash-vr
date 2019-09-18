using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
	/// <summary>
	/// A class to help with scene management.
	/// </summary>
    public class LevelManager : MonoBehaviour
    {
        public void ReloadCurrentScene()
        {
            StartCoroutine(ReloadCurrentSceneRoutine());
        }

        private IEnumerator ReloadCurrentSceneRoutine()
        {
            yield return new WaitForSeconds(4f);

			AdvertManager.instance.IncreaseAdvertCounter();

            if (AdvertManager.instance.CanShowAdvert())
            {
				AdvertManager.instance.ShowAdvert();
				AdvertManager.instance.ResetAdvertCounter();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
