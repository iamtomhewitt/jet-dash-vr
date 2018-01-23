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
    public Text VRCountdownText;
    public GameObject mainMenu;

	private GameSettings gameSettings;
    private string gameSceneName;

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

    public void VRHeadsetCountdown()
    {
        if (gameSettings.isVRMode)
            StartCoroutine(HeadsetCountdown());
        else
            Play(gameSceneName);
    }


    public void SetPlaySceneName(string n)
    {
        gameSceneName = n;
    }

    IEnumerator HeadsetCountdown()
    {
        mainMenu.SetActive(false);
        int countdown = 10;

        for (int i = countdown; i > 0; i--)
        {
            VRCountdownText.text = "PUT ON YOUR VR HEADSET \n" + i;
            yield return new WaitForSeconds(.8f);
        }

        VRCountdownText.text = "";

        Play(gameSceneName);
    }
}
