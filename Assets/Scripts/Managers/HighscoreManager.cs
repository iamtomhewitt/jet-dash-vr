using Achievements;
using Highscore;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;
using UI;
using UnityEngine.Networking;
using UnityEngine;
using Utility;

namespace Manager
{
	/// <summary>
	/// Responsible for uploading, downloading, and saving in game scores.
	/// </summary>
	public class HighscoreManager : MonoBehaviour
	{
		public static HighscoreManager instance;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this.gameObject);
				return;
			}

			DontDestroyOnLoad(this.gameObject);
		}

		/// <summary>
		/// Saves the highscore to PlayerPrefs. Returns true or false if there was a new highscore.
		/// </summary>
		public bool SaveLocalHighscore(int score)
		{
			int currentHighscore = GetLocalHighscore();

			if (score > currentHighscore)
			{
				Debug.Log("New highscore of " + score + "! Saving...");
				PlayerPrefs.SetInt(PlayerPrefKeys.HIGHSCORE, score);

				// Was the highscore achieved in VR mode?
				PlayerPrefs.SetInt(PlayerPrefKeys.WAS_VR_HIGHSCORE, GameSettingsManager.instance.vrMode() ? Constants.YES : Constants.NO);

				// Player has got a new highscore, which hasn't been uploaded yet, so set it to false (0)
				PlayerPrefs.SetInt(PlayerPrefKeys.UPLOADED, Constants.NO);
				PlayerPrefs.SetInt(PlayerPrefKeys.NEW_HIGHSCORE, Constants.YES);

				AchievementManager.instance.UnlockAchievement(AchievementIds.NEW_HIGHSCORE);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Saves the players distance to the PlayerPrefs. Returns true or false if there was a new highscore.
		/// </summary>
		public bool SaveDistanceHighscore(int distance)
		{
			int currentDistance = PlayerPrefs.GetInt(PlayerPrefKeys.DISTANCE);
			if (distance > currentDistance)
			{
				Debug.Log("New distance of " + distance + "! Previous was " + currentDistance + ".");
				PlayerPrefs.SetInt(PlayerPrefKeys.DISTANCE, distance);
				PlayerPrefs.SetInt(PlayerPrefKeys.NEW_DISTANCE, Constants.YES);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Uploads a new highscore to Dreamlo.
		/// </summary>
		public void UploadHighscoreToDreamlo(string username)
		{
			StartCoroutine(UploadHighscoreRoutine(username, PlayerPrefKeys.LEADERBOARD_SCORE));
			StartCoroutine(UploadHighscoreRoutine(username, PlayerPrefKeys.LEADERBOARD_DISTANCE));
		}

		/// <summary>
		/// Routine for uploading a highscore to Dreamlo.
		/// </summary>
		private IEnumerator UploadHighscoreRoutine(string username, string leaderboard)
		{
			bool usedVR = PlayerPrefs.GetInt(PlayerPrefKeys.WAS_VR_HIGHSCORE).Equals(Constants.YES) ? true : false;
			int score = leaderboard.Equals(PlayerPrefKeys.LEADERBOARD_SCORE) ? GetLocalHighscore() : GetBestDistance();
			string shipName = ShopManager.instance.GetSelectedShipData().GetShipName();
			string privateCode = Config.instance.GetConfig()["dreamlo"][leaderboard]["privateKey"];

			string url = Constants.DREAMLO_URL +
						privateCode +
						"/add/" +
						username +
						"/" +
						score +
						"/0/" +
						shipName +
						"|" +
						usedVR;

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();

			if (!request.downloadHandler.text.StartsWith("ERROR"))
			{
				Debug.Log("Upload successful! " + request.responseCode);
				PlayerPrefs.SetInt(PlayerPrefKeys.UPLOADED, Constants.YES);
				PlayerPrefs.SetInt(leaderboard.Equals(PlayerPrefKeys.LEADERBOARD_SCORE) ? PlayerPrefKeys.NEW_HIGHSCORE : PlayerPrefKeys.NEW_DISTANCE, Constants.NO);
				AchievementManager.instance.UnlockAchievement(AchievementIds.UPLOAD_HIGHSCORE);
				NotificationIcon icon = FindObjectOfType<NotificationIcon>();
				if (icon != null)
				{
					icon.TurnOff();
				}
			}
			else
			{
				Debug.Log("Error uploading: " + request.downloadHandler.text);
				HighscoreDisplayHelper display = FindObjectOfType<HighscoreDisplayHelper>();
				display.ClearEntries();
				display.DisplayError(Ui.HIGHSCORE_UPLOAD_ERROR(request.downloadHandler.text));
			}
		}

		/// <summary>
		/// Downloads the highscores from Dreamlo.
		/// </summary>
		public void RequestDownloadOfHighscores()
		{
			StartCoroutine(DownloadAndDisplayHighscores());
		}

		private IEnumerator DownloadAndDisplayHighscores()
		{
			HighscoreDisplayHelper displayHelper = FindObjectOfType<HighscoreDisplayHelper>();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				displayHelper.DisplayError(Ui.NO_INTERNET);
				yield break;
			}

			string url = Config.instance.GetConfig()["firebase"];

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				Debug.Log("Error downloading: " + request.downloadHandler.text);
				displayHelper.DisplayError(Ui.HIGHSCORE_DOWNLOAD_ERROR(request.downloadHandler.text));
			}
			else
			{
				JSONNode json = JSON.Parse(request.downloadHandler.text);
				List<Highscore> highscores = new List<Highscore>();

				foreach (JSONNode i in json)
				{
					highscores.Add(new Highscore(i["name"], i["score"]));
				}

				foreach (Highscore x in highscores)
				{
					Debug.Log(x.ToString());
				}

				highscores.Sort((p1, p2) => p2.GetScore().CompareTo(p1.GetScore()));
				print("Sorted:");

				foreach (Highscore x in highscores)
				{
					Debug.Log(x.ToString());
				}

				displayHelper.DisplayHighscores(highscores);
			}
		}

		public int GetLocalHighscore()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.HIGHSCORE);
		}

		public int GetBestDistance()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.DISTANCE);
		}
	}

	public class Highscore
	{
		private string name;
		private int score;

		public Highscore(string name, int score)
		{
			this.name = name;
			this.score = score;
		}

		public override string ToString()
		{
			return this.name + " | " + this.score;
		}

		public int GetScore()
		{
			return score;
		}

		public string GetName()
		{
			return name;
		}
	}
}