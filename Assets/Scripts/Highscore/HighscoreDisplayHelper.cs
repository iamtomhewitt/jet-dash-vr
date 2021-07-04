using Manager;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace Highscores
{
	public class HighscoreDisplayHelper : MonoBehaviour
	{
		[SerializeField] private Button toggleButton;
		[SerializeField] private GameObject uploadModal;
		[SerializeField] private HighscoreEntry entryPrefab;
		[SerializeField] private Text bestDistanceText;
		[SerializeField] private Text bestScoreText;
		[SerializeField] private Text statusText;
		[SerializeField] private Text scoreHeadingText;
		[SerializeField] private Transform leaderboardContent;

		private List<Highscore> highscores;
		private bool sortByScore = true;

		private void Start()
		{
			bestScoreText.SetText(HighscoreManager.instance.GetLocalHighscore().ToString());
			bestDistanceText.SetText(HighscoreManager.instance.GetBestDistance().ToString());

			statusText.SetText(Ui.DOWNLOADING_HIGHSCORES);
			statusText.color = Color.green;

			HighscoreManager.instance.RequestDownloadOfHighscores();

			HideUploadModal();
			UpdateScoreTypeTexts();
		}

		public void DisplayHighscores(List<Highscore> highscores)
		{
			this.highscores = highscores;

			statusText.SetText("");
			ClearEntries();

			for (int i = 0; i < highscores.Count; i++)
			{
				int rank = i + 1;
				HighscoreEntry entry = Instantiate(entryPrefab, leaderboardContent).GetComponent<HighscoreEntry>();
				entry.Populate(rank, "", "");

				if (this.highscores.Count > i)
				{
					Highscore highscore = this.highscores[i];
					string value = this.sortByScore ? highscore.GetScore().ToString() : highscore.GetDistance().ToString();

					entry.Populate(rank, highscore.GetName(), value);
					entry.SetIcons(highscore.GetShip(), highscore.isVrMode());
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
				HighscoreManager.instance.UploadHighscore(formatted);
				usernameInput.SetText("");
				placeholderText.SetText(Ui.UPLOADED);
			}
		}

		public void ClearEntries()
		{
			foreach (Transform child in leaderboardContent)
			{
				Destroy(child.gameObject);
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

		private void SortByScore()
		{
			this.highscores.Sort((p1, p2) => p2.GetScore().CompareTo(p1.GetScore()));
			this.sortByScore = true;
			this.DisplayHighscores(this.highscores);
		}

		private void SortByDistance()
		{
			this.highscores.Sort((p1, p2) => p2.GetDistance().CompareTo(p1.GetDistance()));
			this.sortByScore = false;
			this.DisplayHighscores(this.highscores);
		}

		public void ToggleScoreType()
		{
			this.sortByScore = !this.sortByScore;
			UpdateScoreTypeTexts();

			if (this.sortByScore == true)
			{
				SortByScore();
			}
			else
			{
				SortByDistance();
			}
		}

		private void UpdateScoreTypeTexts()
		{
			this.toggleButton.GetComponentInChildren<Text>().SetText(this.sortByScore ? "Show Distance" : "Show Score");
			this.scoreHeadingText.SetText(this.sortByScore ? "Score" : "Distance");
		}
	}
}