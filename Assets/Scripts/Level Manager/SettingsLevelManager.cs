using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace LevelManagers
{
	public class SettingsLevelManager : LevelManager
	{
		[SerializeField] private Text toggleVrText;
		[SerializeField] private Slider sensitivitySlider;

		private void Start()
		{
			toggleVrText.text = "VR Mode: " + (GameSettings.instance.vrMode() ? "ON" : "OFF");
			sensitivitySlider.value = GameSettings.instance.GetSensitivity();
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
		public void UpdateSensitivity()
		{
			GameSettings.instance.SetSensitivity(sensitivitySlider.value);
		}
	}
}
