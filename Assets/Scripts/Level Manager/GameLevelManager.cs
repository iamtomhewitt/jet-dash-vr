using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Manager;

namespace LevelManagers
{
	/// <summary>
	/// Handles everything on the Game scene.
	/// </summary>
	public class GameLevelManager : LevelManager
	{
		[SerializeField] private float levelRestartDelay = 1.5f;

		/// <summary>
		/// Restarts the level after the player has died and the scoreboard is shown.
		/// </summary>
		public void RestartLevel()
		{
			StartCoroutine(RestartLevelRoutine());
		}

		private IEnumerator RestartLevelRoutine()
		{
			yield return new WaitForSeconds(levelRestartDelay);

			AdvertManager.instance.IncreaseAdvertCounter();

			if (AdvertManager.instance.CanShowAdvert())
			{
				AdvertManager.instance.ShowAdvert();
				AdvertManager.instance.ResetAdvertCounter();
			}

			this.LoadLevel(SceneManager.GetActiveScene().name);
		}

		public void ReturnToMenu(string sceneName)
		{
			ScreenManager.MakePortrait();
			AudioManager.instance.Pause(SoundNames.SCORE);
			AudioManager.instance.Pause(SoundNames.SHIP_ENGINE);
			this.LoadLevel(sceneName);
		}
	}
}