using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utilities;

namespace Manager
{
	public class HighscoreManager : MonoBehaviour
	{
		public Highscore[] highscoresList;
		public HighscoreEntry[] entries;
		public Text localHighscoreText;
		public Text statusText;
		public InputField usernameInputField;

		private void Start()
		{
			localHighscoreText.text = "Local Highscore: " + LoadLocalHighscore();
			statusText.text = "";

			InitialiseLeaderboard();

			InvokeRepeating("Refresh", 0f, 15f);

			// Debug, resets score
			//PlayerPrefs.SetInt(GameSettings.uploadedKey, 0);
			//PlayerPrefs.SetInt(GameSettings.playerPrefsKey, 1);
		}


		/// <summary>
		/// Convience method to periodically download and refresh the highscores.
		/// </summary>
		private void Refresh()
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
				PlayerPrefs.SetInt(GameSettings.playerPrefsKey, score);

				// Player has got a new highscore, which obvs hasnt been uploaded yet
				PlayerPrefs.SetInt(GameSettings.uploadedKey, 0);
			}
		}


		/// <summary>
		/// Loads the local highscore using PlayerPrefs.
		/// </summary>
		private int LoadLocalHighscore()
		{
			return PlayerPrefs.GetInt(GameSettings.playerPrefsKey);
		}


		/// <summary>
		/// Downloads the highscores from the website, and formats it.
		/// </summary>
		/// <returns></returns>
		private IEnumerator DownloadHighscores()
		{
			UnityWebRequest request = UnityWebRequest.Get(GameSettings.url + GameSettings.publicCode + "/pipe/0/10");
			yield return request.SendWebRequest();

			if (string.IsNullOrEmpty(request.error))
			{
				if (request.downloadHandler.text.StartsWith("ERROR"))
				{
					ClearLeaderboard();
					statusText.text = "Could not get highscores:\n" + request.downloadHandler.text;
				}

				else
				{
					highscoresList = ToHighscoreList(request.downloadHandler.text);
					statusText.text = "";
				}
			}
			else
			{
				print("Error downloading: " + request.error);
				ClearLeaderboard();
				statusText.text = "Could not get highscores:\n" + request.error + "\n" + "Are you connected to the internet?";
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
			else if (PlayerPrefs.GetInt("hasUploadedHighscore") != 0)
			{
				usernameInputField.text = "";
				usernameInputField.placeholder.GetComponent<Text>().text = "Already uploaded!";
			}

			// Otherwise, upload the score
			else
			{
				string id = System.DateTime.Now.ToString("MMddyyyyhhmmss");

				WWW www = new WWW(GameSettings.url + GameSettings.privateCode + "/add/" + WWW.EscapeURL(id + username) + "/" + localHighscore);
				yield return www;

				if (string.IsNullOrEmpty(www.error))
				{
					print("Upload successful!");
					statusText.text = "";
					PlayerPrefs.SetInt(GameSettings.uploadedKey, 1);
				}
				else
				{
					print("Error uploading: " + www.error);
					statusText.text = "Error uploading:\n"+www.error;
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

			for (int i = 0; i < entries.Length; i++)
			{
				string username = highscoresList[i].username;
				int score = highscoresList[i].score;

				entries[i].SetRank(i + 1 + ".");

				if (highscoresList.Length > i)
				{
					//highscoreEntries[i].text += username + " - " + score;
					entries[i].SetName(username);
					entries[i].SetScore(score);
				}
			}
		}


		/// <summary>
		/// Coverts the raw data from the website to a Highscore list.
		/// </summary>
		private Highscore[] ToHighscoreList(string textStream)
		{
			string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

			highscoresList = new Highscore[entries.Length];

			for (int i = 0; i < entries.Length; i++)
			{
				string[] entryInfo = entries[i].Split(new char[] { '|' });

				string username = entryInfo[0].Replace('+', ' ');
				username = username.Substring(14, (username.Length - 14)); // Dont want the date included in the username, so substring itself to show only the username

				int score = int.Parse(entryInfo[1]);

				highscoresList[i] = new Highscore(username, score);

				//print(highscoresList[i].username + ": " + highscoresList[i].score + " Date: "+date);
			}

			return highscoresList;
		}


		/// <summary>
		/// Populates the leaderboard with some placeholders.
		/// </summary>
		private void InitialiseLeaderboard()
		{
			for (int i = 0; i < entries.Length; i++)
			{
				entries[i].SetName("Fetching");
				entries[i].SetScore(0);
				entries[i].SetRank(i + 1 + ".");
			}
		}


		/// <summary>
		/// Sets everything in the leadboard to a nothing value.
		/// </summary>
		private void ClearLeaderboard()
		{
			for (int i = 0; i < entries.Length; i++)
			{
				entries[i].SetName("");
				entries[i].SetScore(0);
				entries[i].SetRank(i + 1 + ".");
			}
		}
	}

	/// <summary>
	/// Data class for keeping track of downloaded highscores.
	/// </summary>
	public struct Highscore
	{
		public string username;
		public int score;

		public Highscore(string username, int score)
		{
			this.username = username;
			this.score = score;
		}
	}	
}
