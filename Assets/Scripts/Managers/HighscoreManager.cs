using Highscore;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
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

			//PlayerPrefs.DeleteAll();
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

				// Player has got a new highscore, which hasn't been uploaded yet, so set it to false (0)
				PlayerPrefs.SetInt(Constants.UPLOADED_KEY, Constants.NO);
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
			UnityWebRequest request = UnityWebRequest.Post(Constants.DREAMLO_URL + Constants.DREAMLO_PRIVATE_CODE + "/add/" + username + "/" + GetLocalHighscore(), "");
			yield return request.SendWebRequest();

			if (!request.downloadHandler.text.StartsWith("ERROR"))
			{
				Debug.Log("Upload successful! " + request.responseCode);
				PlayerPrefs.SetInt(Constants.UPLOADED_KEY, Constants.YES);
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
				string json = JsonHelper.StripParentFromJson(request.downloadHandler.text, 2);
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
