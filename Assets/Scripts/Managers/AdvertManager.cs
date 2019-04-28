using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Manager
{
	public class AdvertManager : MonoBehaviour
	{
		public int advertCounter = 0;
		private string gameID = "1606552";
		private static AdvertManager instance;

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
			Advertisement.Initialize(gameID, true);
		}


		public void ShowAdvert()
		{
			StartCoroutine(ShowAdvertWhenReady());
		}


		private IEnumerator ShowAdvertWhenReady()
		{
			while (!Advertisement.IsReady())
				yield return null;

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
				yield return null;

			Time.timeScale = currentTimeScale;
		}
	}
}
