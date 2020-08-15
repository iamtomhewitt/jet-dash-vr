using UnityEngine;
using Utility;

namespace UI
{
	public class NotificationIcon : MonoBehaviour
	{
		public void Start()
		{
			gameObject.SetActive(IsNewHighscore());
		}

		private bool IsNewHighscore()
		{
			bool newHighscore = PlayerPrefs.GetInt(PlayerPrefKeys.NEW_HIGHSCORE).Equals(Constants.YES);
			bool newDistance = PlayerPrefs.GetInt(PlayerPrefKeys.NEW_DISTANCE).Equals(Constants.YES);
			return newHighscore || newDistance;
		}

		public void TurnOn()
		{
			gameObject.SetActive(true);
		}

		public void TurnOff()
		{
			gameObject.SetActive(false);
		}
	}
}
