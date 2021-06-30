using Manager;
using SimpleJSON;
using System.Collections.Generic;
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
			bestScoreText.SetText(HighscoreManager.instance.GetLocalHighscore().ToString());
			bestDistanceText.SetText(HighscoreManager.instance.GetBestDistance().ToString());

			statusText.SetText(Ui.DOWNLOADING_HIGHSCORES);
			statusText.color = Color.green;

			HighscoreManager.instance.RequestDownloadOfHighscores();

			HideUploadModal();
		}

		public void DisplayHighscores(List<Manager.Highscore> highscores)
		{
			// TODO instead of switching out the parent, should just reorder the list based on what button was pressed
			// Transform parent = leaderboard.Equals(PlayerPrefKeys.LEADERBOARD_SCORE) ? scoreLeaderboardContent : distanceLeaderboardContent;
			Transform parent = scoreLeaderboardContent;

			statusText.SetText("");

			for (int i = 0; i < highscores.Count; i++)
			{
				int rank = i + 1;
				HighscoreEntry entry = Instantiate(entryPrefab, parent).GetComponent<HighscoreEntry>();
				entry.Populate(rank, "", "");

				if (highscores.Count > i)
				{
					entry.Populate(rank, highscores[i].GetName(), highscores[i].GetScore().ToString());
					// TODO
					// entry.SetIcons(highscores[i]["text"]);
					entry.SetTextColourBasedOnRank(rank);
				}
			}
		}

		public void DisplayError(string message)
		{
			ClearEntries();
			statusText.SetText(message);
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
				usernameInput.SetText("");
				placeholderText.SetText(Ui.SCORE_NOT_ZERO);
			}
			else if (string.IsNullOrEmpty(usernameInput.text))
			{
				usernameInput.SetText("");
				placeholderText.SetText(Ui.ENTER_NICKNAME);
			}
			else if (PlayerPrefs.GetInt(PlayerPrefKeys.UPLOADED) != Constants.NO)
			{
				usernameInput.SetText("");
				placeholderText.SetText(Ui.ALREADY_UPLOADED);
			}
			else if (string.IsNullOrEmpty(formatted))
			{
				usernameInput.SetText("");
				placeholderText.SetText(Ui.INVALID_NAME);
			}
			else
			{
				HighscoreManager.instance.UploadHighscoreToDreamlo(formatted);
				usernameInput.SetText("");
				placeholderText.SetText(Ui.UPLOADED);
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