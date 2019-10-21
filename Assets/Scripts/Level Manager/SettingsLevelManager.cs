using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace LevelManagers
{
	public class SettingsLevelManager : LevelManager
	{
		public Text toggleVrText;

		private void Start()
		{
			toggleVrText.text = "VR Mode: " + (GameSettings.instance.vrMode() ? "ON" : "OFF");
		}

		/// <summary>
		/// Toggles the VR mode for the menu display and game settings.
		/// </summary>
		public void ToggleVR()
		{
			bool isVrMode = GameSettings.instance.vrMode();
			GameSettings.instance.SetVrMode(!isVrMode);

			toggleVrText.text = "VR Mode: " + (GameSettings.instance.vrMode() ? "ON" : "OFF");
		}

		/// <summary>
		/// Called from the slider when the value is changed.
		/// </summary>
		public void UpdateSensitivity(Slider slider)
		{
			GameSettings.instance.SetSensitivity(slider.value);
			GameSettings.instance.SetVrMode(false);
		}
	}
}
