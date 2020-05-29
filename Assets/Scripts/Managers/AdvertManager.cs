using Achievements;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;
using Utility;

namespace Manager
{
	public class AdvertManager : MonoBehaviour
	{
		private int advertCounter = 0;
		private int advertsWatched = 0;

		private const int ADVERTS_TO_WATCH_FOR_ACHIEVEMENT = 3;
		private const int REPLAYS_UNTIL_ADVERT = 4;

		public static AdvertManager instance;

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
			string id = Config.instance.GetConfig()["gameId"];
			Advertisement.Initialize(id, false);
		}

		public void ShowAdvert()
		{
			StartCoroutine(ShowAdvertWhenReady());
		}

		private IEnumerator ShowAdvertWhenReady()
		{
			while (!Advertisement.IsReady())
			{
				yield return null;
			}

			Advertisement.Show();

			#if UNITY_EDITOR
			yield return StartCoroutine(WaitForAd());
			#endif

			advertsWatched++;

			AchievementManager.instance.ProgressAchievement(AchievementIds.WATCH_ADS, ADVERTS_TO_WATCH_FOR_ACHIEVEMENT, advertsWatched);
		}

		private IEnumerator WaitForAd()
		{
			float currentTimeScale = Time.timeScale;
			Time.timeScale = 0f;
			yield return null;

			while (Advertisement.isShowing)
			{
				yield return null;
			}

			Time.timeScale = currentTimeScale;
		}

		public void IncreaseAdvertCounter()
		{
			advertCounter++;
		}

		public void ResetAdvertCounter()
		{
			advertCounter = 0;
		}

		public int GetAdvertCounter()
		{
			return advertCounter;
		}

		public bool CanShowAdvert()
		{
			return advertCounter >= REPLAYS_UNTIL_ADVERT;
		}
	}
}