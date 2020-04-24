﻿using UnityEngine;
using UnityEngine.UI;
using Manager;
using Utility;
using SimpleJSON;

namespace Highscore
{
	public class HighscoreDisplayHelper : MonoBehaviour
	{
		[SerializeField] private HighscoreEntry entryPrefab;
		[SerializeField] private Transform entriesParent;
		[SerializeField] private GameObject uploadModal;
		[SerializeField] private Text localHighscoreText;
		[SerializeField] private Text bestDistanceText;
		[SerializeField] private Text statusText;

		private void Start()
		{
			localHighscoreText.text = "Local Highscore: " + HighscoreManager.instance.GetLocalHighscore();
			bestDistanceText.text = "Best Distance: " + HighscoreManager.instance.GetBestDistance();

			statusText.text = "Downloading Highscores...";
			statusText.color = Color.green;

			HighscoreManager.instance.RequestDownloadOfHighscores();

			HideUploadModal();
		}

		public void DisplayHighscores(JSONArray entries)
		{
			statusText.text = "";

			for (int i = 0; i < entries.Count; i++)
			{
				int rank = i + 1;
				HighscoreEntry entry = Instantiate(entryPrefab, entriesParent).GetComponent<HighscoreEntry>();
				entry.Populate(rank + ".", "", "");

				if (entries.Count > i)
				{
					entry.Populate(rank + ".", entries[i]["name"], entries[i]["score"]);
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
			foreach (Transform child in entriesParent)
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
