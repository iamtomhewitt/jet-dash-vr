using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Utility;
using Highscore;
using Achievements;
using SimpleJSON;

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

		public int GetLocalHighscore()
		{
			return PlayerPrefs.GetInt(Constants.HIGHSCORE_KEY);
		}

		public int GetBestDistance()
		{
			return PlayerPrefs.GetInt(Constants.DISTANCE_KEY);
		}

		/// <summary>
		/// Uploads a new highscore to Dreamlo.
		/// </summary>
		public void UploadHighscoreToDreamlo(string username)
		{
			StartCoroutine(UploadHighscoreRoutine(username));
		}

		/// <summary>
		/// Routine for uploading a highscore to Dreamlo.
		/// </summary>
		private IEnumerator UploadHighscoreRoutine(string username)
		{
			// Add '(VR)' to the end of the username if the score was achieved in VR mode
			username = PlayerPrefs.GetInt(Constants.WAS_VR_HIGHSCORE_KEY) == Constants.YES ? username + " (VR)" : username;

			string privateCode = Config.instance.GetConfig()["dreamlo"]["privateKey"];
			
			UnityWebRequest request = UnityWebRequest.Post(Constants.DREAMLO_URL + privateCode + "/add/" + username + "/" + GetLocalHighscore(), "");
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
			StartCoroutine(DownloadScoreHighscores("score"));
			StartCoroutine(DownloadScoreHighscores("distance"));
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
	}
}
