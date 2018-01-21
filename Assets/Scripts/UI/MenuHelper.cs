using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHelper : MonoBehaviour 
{
	public bool VRMode;
	public Image VRModeImage;
	public Slider sensitivitySlider;

	private GameSettings gameSettings;

	void Start()
	{
		gameSettings = GameObject.FindObjectOfType<GameSettings> ();

		// Reset GameSettings values
		gameSettings.isVRMode = VRMode;
		VRModeImage.enabled = VRMode;
		gameSettings.sensitivity = sensitivitySlider.value;
	}

	public void SwitchMode()
	{
		VRMode = !VRMode;
		VRModeImage.enabled = !VRModeImage.enabled;
		gameSettings.isVRMode = VRMode;
	}

	public void Play(string scene)
	{		
		SceneManager.LoadScene (scene);
	}

	public void SetSensitivity()
	{
		gameSettings.sensitivity = sensitivitySlider.value;
	}
}
