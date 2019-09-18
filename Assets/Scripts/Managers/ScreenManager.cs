using UnityEngine;

namespace Manager
{
	/// <summary>
	/// Controls screen orientation.
	/// </summary>
	public class ScreenManager : MonoBehaviour
	{
		public static ScreenManager instance;

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

		public void MakePortrait()
		{
			Screen.orientation = ScreenOrientation.Portrait;
		}

		public void MakeLandscape()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
	}
}
