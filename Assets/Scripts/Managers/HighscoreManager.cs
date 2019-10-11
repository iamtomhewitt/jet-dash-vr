using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Utility;
using Highscore;
using Achievement;

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

			// PlayerPrefs.DeleteAll();
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
				PlayerPrefs.SetInt(Constants.WAS_VR_HIGHSCORE_KEY, GameSettings.instance.vrMode() ? Constants.YES : Constants.NO);

				// Player has got a new highscore, which hasn't been uploaded yet, so set it to false (0)
				PlayerPrefs.SetInt(Constants.UPLOADED_KEY, Constants.NO);

				AchievementManager.instance.UnlockAchievement(AchievementIds.NEW_HIGHSCORE);
			}
		}

		public int GetLocalHighscore()
		{
			return PlayerPrefs.GetInt(Constants.HIGHSCORE_KEY);
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

			UnityWebRequest request = UnityWebRequest.Post(Constants.DREAMLO_URL + Constants.DREAMLO_PRIVATE_CODE + "/add/" + username + "/" + GetLocalHighscore(), "");
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
			StartCoroutine(DownloadHighscores());
		}

		private IEnumerator DownloadHighscores()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				FindObjectOfType<HighscoreDisplayHelper>().DisplayError("No internet connection.");
				yield break;
			}

			UnityWebRequest request = UnityWebRequest.Get(Constants.DREAMLO_URL + Constants.DREAMLO_PUBLIC_CODE + "/json/0/10");
			yield return request.SendWebRequest();

			if (!request.downloadHandler.text.StartsWith("ERROR"))
			{
				string json = HighscoreJsonHelper.StripParentFromJson(request.downloadHandler.text, 2);
				Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(json);
				FindObjectOfType<HighscoreDisplayHelper>().DisplayHighscores(leaderboard);
			}
			else
			{
				Debug.Log("Error downloading: " + request.downloadHandler.text);
				FindObjectOfType<HighscoreDisplayHelper>().DisplayError("Could not download highscores. Please try again later.\n\n" + request.downloadHandler.text);
			}
		}
	}
}
