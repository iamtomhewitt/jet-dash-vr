using Achievements;
using Highscores;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UI;
using UnityEngine.Networking;
using UnityEngine;
using Utility;

namespace Manager
{
	/// <summary>
	/// Responsible for uploading, downloading, and saving in game scores.
	/// </summary>
	public class HighscoreManager : MonoBehaviour
	{
		public static HighscoreManager instance;
		private string datetime;

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
		/// Saves the highscore to PlayerPrefs. Returns true or false if there was a new highscore.
		/// </summary>
		public bool SaveLocalHighscore(int score)
		{
			int currentHighscore = GetLocalHighscore();

			if (score > currentHighscore)
			{
				Debug.Log("New highscore of " + score + ", previous highscore was: " + currentHighscore);
				PlayerPrefs.SetInt(PlayerPrefKeys.HIGHSCORE, score);

				// Was the highscore achieved in VR mode?
				PlayerPrefs.SetInt(PlayerPrefKeys.WAS_VR_HIGHSCORE, GameSettingsManager.instance.vrMode() ? Constants.YES : Constants.NO);

				// Player has got a new highscore, which hasn't been uploaded yet, so set it to false (0)
				PlayerPrefs.SetInt(PlayerPrefKeys.UPLOADED, Constants.NO);
				PlayerPrefs.SetInt(PlayerPrefKeys.NEW_HIGHSCORE, Constants.YES);

				AchievementManager.instance.UnlockAchievement(AchievementIds.NEW_HIGHSCORE);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Saves the players distance to the PlayerPrefs. Returns true or false if there was a new highscore.
		/// </summary>
		public bool SaveDistanceHighscore(int distance)
		{
			int currentDistance = PlayerPrefs.GetInt(PlayerPrefKeys.DISTANCE);
			if (distance > currentDistance)
			{
				Debug.Log("New distance of " + distance + ", previous distance was: " + currentDistance);
				PlayerPrefs.SetInt(PlayerPrefKeys.DISTANCE, distance);
				PlayerPrefs.SetInt(PlayerPrefKeys.NEW_DISTANCE, Constants.YES);
				return true;
			}
			return false;
		}

		public void UploadHighscore(string username)
		{
			StartCoroutine(UploadHighscoreRoutine(username));
		}

		private IEnumerator UploadHighscoreRoutine(string username)
		{
			yield return GetDateFromInternet();

			bool usedVR = PlayerPrefs.GetInt(PlayerPrefKeys.WAS_VR_HIGHSCORE).Equals(Constants.YES);
			string shipName = ShopManager.instance.GetSelectedShipData().GetShipName();
			string url = Config.instance.GetConfig()["firebase"];

			JSONObject body = new JSONObject();
			body.Add("date", this.datetime);
			body.Add("distance", GetBestDistance());
			body.Add("name", username);
			body.Add("score", GetLocalHighscore());
			body.Add("ship", shipName);
			body.Add("vrMode", usedVR);

			UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "POST");
			byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.Log("Error uploading: " + request.downloadHandler.text);
				HighscoreDisplayHelper display = FindObjectOfType<HighscoreDisplayHelper>();
				display.ClearEntries();
				display.DisplayError(Ui.HIGHSCORE_UPLOAD_ERROR(request.downloadHandler.text));
			}
			else
			{
				Debug.Log("Upload successful! " + request.result);
				PlayerPrefs.SetInt(PlayerPrefKeys.UPLOADED, Constants.YES);
				PlayerPrefs.SetInt(PlayerPrefKeys.NEW_HIGHSCORE, Constants.NO);
				PlayerPrefs.SetInt(PlayerPrefKeys.NEW_DISTANCE, Constants.NO);
				AchievementManager.instance.UnlockAchievement(AchievementIds.UPLOAD_HIGHSCORE);
				NotificationIcon[] icons = FindObjectsOfType<NotificationIcon>();
				foreach (NotificationIcon icon in icons)
				{
					icon.TurnOff();
				}
			}
		}

		public void DownloadHighscores()
		{
			StartCoroutine(DownloadAndDisplayHighscores());
		}

		private IEnumerator DownloadAndDisplayHighscores()
		{
			HighscoreDisplayHelper displayHelper = FindObjectOfType<HighscoreDisplayHelper>();

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				displayHelper.DisplayError(Ui.NO_INTERNET);
				yield break;
			}
			string url = Config.instance.GetConfig()["firebase"];

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.Log("Error downloading: " + request.downloadHandler.text);
				displayHelper.DisplayError(Ui.HIGHSCORE_DOWNLOAD_ERROR(request.downloadHandler.text));
			}
			else
			{
				JSONNode json = JSON.Parse(request.downloadHandler.text);
				List<Highscore> highscores = new List<Highscore>();

				foreach (JSONNode i in json)
				{
					highscores.Add(new Highscore(i["name"], i["score"], i["distance"], i["ship"], i["vrMode"], i["date"]));
				}

				highscores.Sort((p1, p2) => p2.GetScore().CompareTo(p1.GetScore()));

				displayHelper.DisplayHighscores(highscores);
			}
		}

		public int GetLocalHighscore()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.HIGHSCORE);
		}

		public int GetBestDistance()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.DISTANCE);
		}

		private IEnumerator GetDateFromInternet() 
		{
			UnityWebRequest request = UnityWebRequest.Get("https://worldtimeapi.org/api/timezone/Europe/London");
			yield return request.SendWebRequest();
			JSONNode json = JSON.Parse(request.downloadHandler.text);
			this.datetime = json["datetime"];
		}
	}
}