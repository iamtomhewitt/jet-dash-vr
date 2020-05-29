using Manager;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace Highscore
{
	public class HighscoreDisplayHelper : MonoBehaviour
	{
		[SerializeField] private GameObject uploadModal;
		[SerializeField] private HighscoreEntry entryPrefab;
		[SerializeField] private Text bestDistanceText;
		[SerializeField] private Text bestScoreText;
		[SerializeField] private Text statusText;
		[SerializeField] private Transform distanceLeaderboardContent;
		[SerializeField] private Transform scoreLeaderboardContent;

		private void Start()
		{
			bestScoreText.text = HighscoreManager.instance.GetLocalHighscore().ToString();
			bestDistanceText.text = HighscoreManager.instance.GetBestDistance().ToString();

			statusText.text = "Downloading Highscores...";
			statusText.color = Color.green;

			HighscoreManager.instance.RequestDownloadOfHighscores();

			HideUploadModal();
		}

		public void DisplayHighscores(JSONArray entries, string leaderboard)
		{
			Transform parent = leaderboard.Equals("score") ? scoreLeaderboardContent : distanceLeaderboardContent;

			statusText.text = "";

			for (int i = 0; i < entries.Count; i++)
			{
				int rank = i + 1;
				HighscoreEntry entry = Instantiate(entryPrefab, parent).GetComponent<HighscoreEntry>();
				entry.Populate(rank, "", "");

				if (entries.Count > i)
				{
					entry.Populate(rank, entries[i]["name"], entries[i]["score"]);
					entry.SetIcons(entries[i]["text"]);
					entry.SetTextColourBasedOnRank(rank);
				}
			}
		}

		public void DisplayError(string message)
		{
			ClearEntries();
			statusText.text = message;
			statusText.color = Color.red;
		}

		/// <summary>
		/// Called from a Unity button, uploads the highscore to Dreamlo.
		/// </summary>
		public void UploadHighscore(InputField usernameInput)
		{
			Text placeholderText = usernameInput.placeholder.GetComponent<Text>();
			string formatted = usernameInput.text.StripNonLatinLetters();

			if (HighscoreManager.instance.GetLocalHighscore() <= 0)
			{
				usernameInput.text = "";
				placeholderText.text = "Score cannot be 0!";
			}
			else if (string.IsNullOrEmpty(usernameInput.text))
			{
				usernameInput.text = "";
				placeholderText.text = "Enter a nickname!";
			}
			else if (PlayerPrefs.GetInt(PlayerPrefKeys.UPLOADED) != Constants.NO)
			{
				usernameInput.text = "";
				placeholderText.text = "Already uploaded!";
			}
			else if (string.IsNullOrEmpty(formatted))
			{
				usernameInput.text = "";
				placeholderText.text = "Invalid name!";
			}
			else
			{
				HighscoreManager.instance.UploadHighscoreToDreamlo(formatted);
				usernameInput.text = "";
				placeholderText.text = "Uploaded!";
			}
		}

		public void ClearEntries()
		{
			foreach (Transform child in scoreLeaderboardContent)
			{
				Destroy(child);
			}
		}

		public void ShowUploadModal()
		{
			uploadModal.SetActive(true);
		}

		public void HideUploadModal()
		{
			uploadModal.SetActive(false);
		}
	}
}