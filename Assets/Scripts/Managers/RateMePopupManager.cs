using UnityEngine;

namespace Manager
{
	public class RateMePopupManager : MonoBehaviour
	{
		[SerializeField] private GameObject popupVertical;
		[SerializeField] private GameObject popupHorizontal;

		private int showCount = 0;
		private const int NUMBER_OF_TIMES_BEFORE_POPUP_SHOW = 25;

		public static RateMePopupManager instance;

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
			popupHorizontal.SetActive(false);
			popupVertical.SetActive(false);
		}

		public void IncreaseShowCount()
		{
			showCount++;
		}

		public bool CanShowRateMePopup()
		{
			if (showCount > NUMBER_OF_TIMES_BEFORE_POPUP_SHOW)
		}

		public void ShowRateMePopup()
			{
				bool isHorizontal = ScreenManager.IsLandScape() ? true : false;
				popupHorizontal.SetActive(isHorizontal);
				popupVertical.SetActive(!isHorizontal);
				showCount = 0;
			}

		public void GoToGooglePlayPage()
		{
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.BlueRobotGames.JetDashVR");
			HidePopup();
			Time.timeScale = 1f;
		}

		public void HidePopup()
		{
			popupHorizontal.SetActive(false);
			popupVertical.SetActive(false);
			Time.timeScale = 1f;
		}
	}
}