using UnityEngine;
using UnityEngine.UI;
using Manager;
using Utility;
using SimpleJSON;

namespace Highscore
{
	public class HighscoreDisplayHelper : MonoBehaviour
	{
		[SerializeField] private HighscoreEntry[] highscoreEntries;
		[SerializeField] private Text localHighscoreText;
		[SerializeField] private Text bestDistanceText;
		[SerializeField] private Text statusText;

		private void Start()
		{
			localHighscoreText.text = "Local Highscore: " + HighscoreManager.instance.GetLocalHighscore();
			bestDistanceText.text = "Best Distance: " + HighscoreManager.instance.GetBestDistance();

			for (int i = 0; i < highscoreEntries.Length; i++)
			{
				highscoreEntries[i].Populate(i + 1 + ".", "Fetching...", "");
			}

			HighscoreManager.instance.RequestDownloadOfHighscores();
		}

		public void DisplayHighscores(JSONArray entries)
		{
			for (int i = 0; i < entries.Count; i++)
			{
				highscoreEntries[i].Populate(i + 1 + ".", "", "");

				if (entries.Count > i)
				{
					highscoreEntries[i].Populate(i + 1 + ".", entries[i]["name"], entries[i]["score"]);
				}
			}
		}

		public void DisplayError(string message)
		{
			ClearEntries();
			statusText.text = message;
		}

		/// <summary>
		/// Called from a Unity button, uploads the highscore to Dreamlo.
		/// </summary>
		public void UploadHighscore(InputField usernameInput)
		{
			Text placeholderText = usernameInput.placeholder.GetComponent<Text>();

			if (HighscoreManager.instance.GetLocalHighscore() <= 0)
			{
				placeholderText.text = "Score cannot be 0!";
			}
			else if (string.IsNullOrEmpty(usernameInput.text))
			{
				placeholderText.text = "Enter a nickname!";
			}
			else if (PlayerPrefs.GetInt(Constants.UPLOADED_KEY) != Constants.NO)
			{
				usernameInput.text = "";
				placeholderText.text = "Already uploaded!";
			}
			else
			{
				HighscoreManager.instance.UploadHighscoreToDreamlo(usernameInput.text);
				usernameInput.text = "";
				placeholderText.text = "Uploaded!";
			}
		}

		public void ClearEntries()
		{
			for (int i = 0; i < highscoreEntries.Length; i++)
			{
				highscoreEntries[i].Populate("", "", "");
			}
		}
	}
}
