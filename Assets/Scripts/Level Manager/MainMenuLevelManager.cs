using Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

/// <summary>
/// Handles everything on the Main Menu.
/// </summary>
public class MainMenuLevelManager : LevelManager
{
	[SerializeField] private Image vrToggleImage;
	[SerializeField] private Slider sensitivitySlider;
	[SerializeField] private Text vrCountdownText;
	[SerializeField] private GameObject mainMenuUi;

	[SerializeField] private bool vrMode;

	public string gameSceneName;

	private void Start()
	{
		GameSettings.instance.SetVrMode(vrMode);
		UpdateSensitivity();
		vrToggleImage.enabled = vrMode;
	}

	/// <summary>
	/// Toggles the VR mode for the menu display and game settings.
	/// </summary>
	public void ToggleVrMode()
	{
		vrMode = !vrMode;
		vrToggleImage.enabled = !vrToggleImage.enabled;
		GameSettings.instance.SetVrMode(vrMode);
	}

	/// <summary>
	/// Called from the slider when the value is changed.
	/// </summary>
	public void UpdateSensitivity()
	{
		GameSettings.instance.SetSensitivity(sensitivitySlider.value);
	}

	/// <summary>
	/// Loads the game scene. Shows the headset countdown if in VR mode.
	/// </summary>
	public void LoadGame()
	{
		if (vrMode)
		{
			StartCoroutine(HeadsetCountdown());
		}
		else
		{
			this.LoadLevel(gameSceneName);
		}
	}

	/// <summary>
	/// Shows the headset message whilst counting down to the game start.
	/// </summary>
	private IEnumerator HeadsetCountdown()
	{
		ScreenManager.MakeLandscape();

		mainMenuUi.SetActive(false);

		int timeRemaining = 10;
		while (timeRemaining > 0)
		{
			vrCountdownText.text = "PUT ON YOUR VR HEADSET \n" + timeRemaining;
			timeRemaining--;
			yield return new WaitForSeconds(1f);
		}

		vrCountdownText.text = "";

		this.LoadLevel(gameSceneName);
	}
}
