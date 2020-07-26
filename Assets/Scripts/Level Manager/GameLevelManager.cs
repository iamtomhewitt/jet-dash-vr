using Manager;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace LevelManagers
{
	/// <summary>
	/// Handles everything on the Game scene.
	/// </summary>
	public class GameLevelManager : LevelManager
	{
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private float levelRestartDelay = 1.5f;

		private AdvertManager advertManager;
		private AudioManager audioManager;

		private const float TIME_NORMAL = 1f;
		private const float TIME_STOPPED = 0f;

		private void Start()
		{
			advertManager = AdvertManager.instance;
			audioManager = AudioManager.instance;
			Time.timeScale = TIME_NORMAL;
		}

		/// <summary>
		/// Restarts the level after the player has died and the scoreboard is shown.
		/// </summary>
		public void RestartLevel()
		{
			StartCoroutine(RestartLevelRoutine());
		}

		public void ShowPauseMenu()
		{
			pauseMenu.SetActive(true);
			Time.timeScale = TIME_STOPPED;
		}

		public void HidePauseMenu()
		{
			pauseMenu.SetActive(false);
			Time.timeScale = TIME_NORMAL;
		}

		private IEnumerator RestartLevelRoutine()
		{
			yield return new WaitForSeconds(levelRestartDelay);

			advertManager.IncreaseAdvertCounter();

			if (advertManager.CanShowAdvert())
			{
				advertManager.ShowAdvert();
				advertManager.ResetAdvertCounter();
			}

			this.LoadLevel(SceneManager.GetActiveScene().name);
		}

		public void ReturnToMenu(string sceneName)
		{
			Time.timeScale = TIME_NORMAL;
			ScreenManager.MakePortrait();
			audioManager.Pause(SoundNames.SCORE);
			audioManager.Pause(SoundNames.SHIP_ENGINE);
			this.LoadLevel(sceneName);
		}
	}
}