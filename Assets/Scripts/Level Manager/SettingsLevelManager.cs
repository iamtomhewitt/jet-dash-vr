using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace LevelManagers
{
	public class SettingsLevelManager : LevelManager
	{
		[SerializeField] private Text toggleVrText;
		[SerializeField] private Slider sensitivitySlider;

		private void Start()
		{
			toggleVrText.text = "VR Mode: " + (GameSettingsManager.instance.vrMode() ? "ON" : "OFF");
			sensitivitySlider.value = GameSettingsManager.instance.GetSensitivity();
		}

		/// <summary>
		/// Toggles the VR mode for the menu display and game settings.
		/// </summary>
		public void ToggleVR()
		{
			bool isVrMode = GameSettingsManager.instance.vrMode();
			GameSettingsManager.instance.SetVrMode(!isVrMode);

			toggleVrText.text = "VR Mode: " + (GameSettingsManager.instance.vrMode() ? "ON" : "OFF");
		}

		/// <summary>
		/// Called from the slider when the value is changed.
		/// </summary>
		public void UpdateSensitivity()
		{
			GameSettingsManager.instance.SetSensitivity(sensitivitySlider.value);
		}
	}
}
