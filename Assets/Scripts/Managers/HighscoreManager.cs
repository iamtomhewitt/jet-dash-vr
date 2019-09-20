using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utilities;
using Highscore;
using Utility;

namespace Manager
{
	/// <summary>
	/// Manages the highscores on the highscore scene. Responsible for uploading, downloading, and displaying scores.
	/// </summary>
	public class HighscoreManager : MonoBehaviour
	{
		[SerializeField] private Text localHighscoreText;
		[SerializeField] private Text statusText;
		[SerializeField] private InputField usernameInputField;
		[SerializeField] private HighscoreEntry[] UIEntries;

		private Leaderboard leaderboard;

		public static HighscoreManager instance;

		private void Awake()
		{
			if (instance)
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
				instance = this;
			}
		}

		private void Start()
		{
			localHighscoreText.text = "Local Highscore: " + LoadLocalHighscore();
			statusText.text = "";

			InitialiseLeaderboard();

			InvokeRepeating("RefreshHighscores", 0f, 15f);

			// Debug, resets score
			//PlayerPrefs.SetInt(Constants.UPLOADED_KEY, 0);
			//PlayerPrefs.SetInt(GameSettings.playerPrefsKey, 1);
		}

		/// <summary>
		/// Convience method to periodically download and refresh the highscores.
		/// </summary>
		private void RefreshHighscores()
		{
			StartCoroutine(DisplayHighScores());
		}

		/// <summary>
		/// Saves the current local highscore.
		/// </summary>
		public void SaveLocalHighscore(int score)
		{
			int currentHighscore = LoadLocalHighscore();

			if (score > currentHighscore)
			{
				print("New highscore of " + score + "! Saving...");
				PlayerPrefs.SetInt(Constants.HIGHSCORE_KEY, score);

				// Player has got a new highscore, which obvs hasnt been uploaded yet
				PlayerPrefs.SetInt(Constants.UPLOADED_KEY, 0);
			}
		}

		/// <summary>
		/// Loads the local highscore using PlayerPrefs.
		/// </summary>
		private int LoadLocalHighscore()
		{
			return PlayerPrefs.GetInt(Constants.HIGHSCORE_KEY);
		}

		/// <summary>
		/// Downloads the highscores from the website, and formats it.
		/// </summary>
		/// <returns></returns>
		private IEnumerator DownloadHighscores()
		{
			UnityWebRequest request = UnityWebRequest.Get(Constants.DREAMLO_URL + Constants.DREAMLO_URL + "/json");
			yield return request.SendWebRequest();

			if (request.downloadHandler.text.StartsWith("ERROR"))
			{
				ClearLeaderboard();
				statusText.text = "Could not get highscores:\n" + request.downloadHandler.text;
			}

			else
			{
				string json = JSONHelper.RemoveNJsonFields(request.downloadHandler.text, 2);
				leaderboard = JsonUtility.FromJson<Leaderboard>(json);
				statusText.text = "";
			}
		}

		/// <summary>
		/// Starts the UploadHighscore Coroutine. This method is used on a Unity Button.
		/// </summary>
		public void Upload()
		{
			StartCoroutine(UploadHighscore());
		}

		/// <summary>
		/// Uploads a new score onto the website, based on the correct conditions.
		/// </summary>
		private IEnumerator UploadHighscore()
		{
			int localHighscore = LoadLocalHighscore();
			string username = usernameInputField.text;

			// If an invalid score
			if (localHighscore <= 0)
			{
				usernameInputField.placeholder.GetComponent<Text>().text = "Score cannot be 0!";
			}

			// If no nickname set
			else if (string.IsNullOrEmpty(usernameInputField.text))
			{
				usernameInputField.placeholder.GetComponent<Text>().text = "Enter a nickname!";
			}

			// If the score has already been uploaded
			else if (PlayerPrefs.GetInt(Constants.UPLOADED_KEY) != Constants.NO)
			{
				usernameInputField.text = "";
				usernameInputField.placeholder.GetComponent<Text>().text = "Already uploaded!";
			}

			// Otherwise, upload the score
			else
			{
				string url = Constants.DREAMLO_URL + Constants.DREAMLO_PRIVATE_CODE + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + localHighscore;
				UnityWebRequest request = UnityWebRequest.Post(url, "");
				yield return request.SendWebRequest();

				if (request.downloadHandler.text.StartsWith("ERROR"))
				{
					ClearLeaderboard();
					statusText.text = "Error uploading:\n" + request.downloadHandler.text;
				}
				else
				{
					print("Upload successful!");
					statusText.text = "";
					PlayerPrefs.SetInt(Constants.UPLOADED_KEY, Constants.YES);
				}

				// And reset the input field
				usernameInputField.text = "";
				usernameInputField.placeholder.GetComponent<Text>().text = "Uploaded!";
			}
		}

		/// <summary>
		/// Updates the UI to show the downloaded scores.
		/// </summary>
		private IEnumerator DisplayHighScores()
		{
			yield return StartCoroutine(DownloadHighscores());

			HighscoreData[] data = leaderboard.entry;

			for (int i = 0; i < UIEntries.Length; i++)
			{
				string username = data[i].name;
				int score = data[i].score;
				string rank = i + 1 + ".";

				UIEntries[i].SetUsernameText(username);
				UIEntries[i].SetRankText(rank);
				UIEntries[i].SetScoreText(score);
			}
		}

		/// <summary>
		/// Populates the leaderboard with some placeholders.
		/// </summary>
		private void InitialiseLeaderboard()
		{
			for (int i = 0; i < UIEntries.Length; i++)
			{
				UIEntries[i].SetUsernameText("Fetching");
				UIEntries[i].SetScoreText(0);
				UIEntries[i].SetRankText(i + 1 + ".");
			}
		}

		/// <summary>
		/// Sets everything in the leadboard to a nothing value.
		/// </summary>
		private void ClearLeaderboard()
		{
			for (int i = 0; i < UIEntries.Length; i++)
			{
				UIEntries[i].SetUsernameText("");
				UIEntries[i].SetScoreText(0);
				UIEntries[i].SetRankText(i + 1 + ".");
			}
		}
	}
}
