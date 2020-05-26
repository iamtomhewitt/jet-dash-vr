using Achievements;
using Highscore;
using SimpleJSON;
using System.Collections;
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
		/// Saves the highscore to PlayerPrefs.
		/// </summary>
		public void SaveLocalHighscore(int score)
		{
			int currentHighscore = GetLocalHighscore();

			if (score > currentHighscore)
			{
				Debug.Log("New highscore of " + score + "! Saving...");
				PlayerPrefs.SetInt(Constants.HIGHSCORE_KEY, score);

				// Was the highscore achieved in VR mode?
				PlayerPrefs.SetInt(Constants.WAS_VR_HIGHSCORE_KEY, GameSettingsManager.instance.vrMode() ? Constants.YES : Constants.NO);

				// Player has got a new highscore, which hasn't been uploaded yet, so set it to false (0)
				PlayerPrefs.SetInt(Constants.UPLOADED_KEY, Constants.NO);

				AchievementManager.instance.UnlockAchievement(AchievementIds.NEW_HIGHSCORE);
			}
		}

		/// <summary>
		/// Uploads a new highscore to Dreamlo.
		/// </summary>
		public void UploadHighscoreToDreamlo(string username)
		{
			StartCoroutine(UploadHighscoreRoutine(username, Constants.LEADERBOARD_SCORE_KEY));
			StartCoroutine(UploadHighscoreRoutine(username, Constants.LEADERBOARD_DISTANCE_KEY));
		}

		/// <summary>
		/// Routine for uploading a highscore to Dreamlo.
		/// </summary>
		private IEnumerator UploadHighscoreRoutine(string username, string leaderboard)
		{
			bool usedVR = PlayerPrefs.GetInt(Constants.WAS_VR_HIGHSCORE_KEY).Equals(Constants.YES) ? true : false;
			int score = leaderboard.Equals(Constants.LEADERBOARD_SCORE_KEY) ? GetLocalHighscore() : GetBestDistance();
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
				PlayerPrefs.SetInt(Constants.UPLOADED_KEY, Constants.YES);
				AchievementManager.instance.UnlockAchievement(AchievementIds.UPLOAD_HIGHSCORE);
			}
			else
			{
				Debug.Log("Error uploading: " + request.downloadHandler.text);
				HighscoreDisplayHelper display = FindObjectOfType<HighscoreDisplayHelper>();
				display.ClearEntries();
				display.DisplayError("Could not upload score. Please try again later.\n\n" + request.downloadHandler.text);
			}
		}

		/// <summary>
		/// Downloads the highscores from Dreamlo.
		/// </summary>
		public void RequestDownloadOfHighscores()
		{
			StartCoroutine(DownloadScoreHighscores(Constants.LEADERBOARD_SCORE_KEY));
			StartCoroutine(DownloadScoreHighscores(Constants.LEADERBOARD_DISTANCE_KEY));
		}

		private IEnumerator DownloadScoreHighscores(string leaderboard)
		{
			HighscoreDisplayHelper displayHelper = FindObjectOfType<HighscoreDisplayHelper>();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				displayHelper.DisplayError("No internet connection.");
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
				displayHelper.DisplayError("Could not download highscores. Please try again later.\n\n" + request.downloadHandler.text);
			}
		}

		public int GetLocalHighscore()
		{
			return PlayerPrefs.GetInt(Constants.HIGHSCORE_KEY);
		}

		public int GetBestDistance()
		{
			return PlayerPrefs.GetInt(Constants.DISTANCE_KEY);
		}
	}
}
