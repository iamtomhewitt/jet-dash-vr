using Manager;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace LevelManagers
{
	public class SettingsLevelManager : LevelManager
	{
		[SerializeField] private Text toggleVrText;
		[SerializeField] private Text sensitivityText;
		[SerializeField] private int maxSensitivity;
		[SerializeField] private int minSensitivity;
		[SerializeField] private float sensitivityStep;

		private GameSettingsManager gameSettings;
		private float sensitivity;

		private void Start()
		{
			gameSettings = GameSettingsManager.instance;
			SetVrToggleText();
			sensitivity = gameSettings.GetSensitivity();
			SetSensitivityText();
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

		public void IncreaseSensitivity()
		{
			sensitivity += sensitivityStep;
			if (sensitivity > maxSensitivity)
			{
				sensitivity = maxSensitivity;
			}
			gameSettings.SetSensitivity(sensitivity);
			SetSensitivityText();
		}

		public void DecreaseSensitivity()
		{
			sensitivity -= sensitivityStep;
			if (sensitivity < minSensitivity)
			{
				sensitivity = minSensitivity;
			}
			gameSettings.SetSensitivity(sensitivity);
			SetSensitivityText();
		}

		private void SetVrToggleText()
		{
			toggleVrText.SetText(Ui.VR_TOGGLE(gameSettings.vrMode()));
		}

		private void SetSensitivityText()
		{
			sensitivityText.SetText(sensitivity.ToString());
		}
	}
}