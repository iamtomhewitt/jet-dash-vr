using UnityEngine;

namespace Manager
{
	/// <summary>
	/// Controls screen orientation.
	/// </summary>
	public class ScreenManager : MonoBehaviour
	{
		public static void MakePortrait()
		{
			Screen.orientation = ScreenOrientation.Portrait;
		}

		public static void MakeLandscape()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}

		public static bool IsLandScape()
		{
			return (Screen.orientation == ScreenOrientation.Landscape ||
			Screen.orientation == ScreenOrientation.LandscapeLeft ||
			Screen.orientation == ScreenOrientation.LandscapeRight);
		}
	}
}