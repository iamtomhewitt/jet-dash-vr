using Manager;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace LevelManagers
{
	public class SettingsLevelManager : LevelManager
	{
		[SerializeField] private Text toggleVrText;
		[SerializeField] private Slider sensitivitySlider;

		private GameSettingsManager gameSettings;

		private void Start()
		{
			gameSettings = GameSettingsManager.instance;
			SetVrToggleText();
			sensitivitySlider.value = gameSettings.GetSensitivity();
		}

		/// <summary>
		/// Toggles the VR mode for the menu display and game settings.
		/// </summary>
		public void ToggleVR()
		{
			bool isVrMode = gameSettings.vrMode();
			gameSettings.SetVrMode(!isVrMode);
			SetVrToggleText();
		}

		/// <summary>
		/// Called from the slider when the value is changed.
		/// </summary>
		public void UpdateSensitivity()
		{
			gameSettings.SetSensitivity(sensitivitySlider.value);
		}

		private void SetVrToggleText()
		{
			toggleVrText.SetText(Ui.VR_TOGGLE(gameSettings.vrMode()));
		}
	}
}