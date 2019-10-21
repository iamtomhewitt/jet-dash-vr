using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Achievements;
using Manager;
using Utility;

namespace LevelManagers
{
	/// <summary>
	/// Handles everything on the Main Menu.
	/// </summary>
	public class MainMenuLevelManager : LevelManager
	{
		[SerializeField] private Text vrCountdownText;
		[SerializeField] private GameObject mainMenuUi;

		public string gameSceneName;

		/// <summary>
		/// Loads the game scene. Shows the headset countdown if in VR mode.
		/// </summary>
		public void LoadGame()
		{
			if (GameSettings.instance.vrMode())
			{
				StartCoroutine(HeadsetCountdown());
				AchievementManager.instance.UnlockAchievement(AchievementIds.PLAY_IN_VR);
			}
			else
			{
				ScreenManager.MakeLandscape();
				this.LoadLevel(gameSceneName);
			}
		}

		/// <summary>
		/// Shows the headset message whilst counting down to the game start.
		/// </summary>
		private IEnumerator HeadsetCountdown()
		{
			ScreenManager.MakeLandscape();

			mainMenuUi.SetActive(false);

			int timeRemaining = 10;
			while (timeRemaining > 0)
			{
				vrCountdownText.text = "PUT ON YOUR VR HEADSET \n" + timeRemaining;
				timeRemaining--;
				yield return new WaitForSeconds(1f);
			}

			vrCountdownText.text = "";

			this.LoadLevel(gameSceneName);
		}
	}
}