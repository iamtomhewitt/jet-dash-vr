using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using Utility;

namespace Manager
{
	public class AdvertManager : MonoBehaviour
	{
		private int advertCounter = 0;

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
			Advertisement.Initialize(Constants.GAME_ID, true);
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
			return advertCounter >= 4;
		}
	}
}
