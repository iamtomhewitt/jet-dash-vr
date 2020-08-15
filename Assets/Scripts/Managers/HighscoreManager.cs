﻿using Achievements;
using Highscore;
using SimpleJSON;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using Utility;
using UI;

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
			StartCoroutine(DownloadScoreHighscores(PlayerPrefKeys.LEADERBOARD_SCORE));
			StartCoroutine(DownloadScoreHighscores(PlayerPrefKeys.LEADERBOARD_DISTANCE));
		}

		private IEnumerator DownloadScoreHighscores(string leaderboard)
		{
			HighscoreDisplayHelper displayHelper = FindObjectOfType<HighscoreDisplayHelper>();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				displayHelper.DisplayError(Ui.NO_INTERNET);
				yield break;
			}

			string publicCode = Config.instance.GetConfig()["dreamlo"][leaderboard]["publicKey"];
			UnityWebRequest request = UnityWebRequest.Get(Constants.DREAMLO_URL + publicCode + "/json");
			yield return request.SendWebRequest();

			if (!request.downloadHandler.text.StartsWith("ERROR"))
			{
				JSONNode json = JSON.Parse(request.downloadHandler.text);
				JSONArray entries = json["dreamlo"]["leaderboard"]["entry"].AsArray;
				displayHelper.DisplayHighscores(entries, leaderboard);
			}
			else
			{
				Debug.Log("Error downloading: " + request.downloadHandler.text);
				displayHelper.DisplayError(Ui.HIGHSCORE_DOWNLOAD_ERROR(request.downloadHandler.text));
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
}