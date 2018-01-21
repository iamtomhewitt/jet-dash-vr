using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public 	float speed;
	public 	float speedIncrease;
	public 	float speedIncreaseRepeatRate;
	public 	float turningSpeed;

    public 	GameObject normalCamera;
	public 	GameObject VRCamera;
	private GameObject cameraUsed;
	private HUD hud;
	private float sensitivity;

	public 	KeyCode left;
	public 	KeyCode right;

    [Space()]
    public ModelSettings modelSettings;

	void Start ()
	{
		hud = GetComponent<HUD> ();
		hud.speedText.text = speed.ToString ();

        modelSettings.originalRotation = modelSettings.model.rotation;

		InvokeRepeating ("IncreaseSpeed", speedIncreaseRepeatRate, speedIncreaseRepeatRate);

		GameSettings gs = GameObject.FindObjectOfType<GameSettings> ();

		if (gs == null) 
		{
			normalCamera.SetActive (true);
			VRCamera.SetActive (false);
			cameraUsed = normalCamera;
			sensitivity = 1f;
		}
		else if (gs.isVRMode) 
		{
			normalCamera.SetActive (false);
			VRCamera.SetActive (true);
			cameraUsed = VRCamera;
			sensitivity = gs.sensitivity;
		} 
		else 
		{
			normalCamera.SetActive (true);
			VRCamera.SetActive (false);
			cameraUsed = normalCamera;
			sensitivity = gs.sensitivity;
		}
	}

	void Update ()
	{
        // Move forward
		transform.position += transform.forward * Time.deltaTime * speed;

        // Move left and right based on accelerometer
		transform.position += transform.right * Time.deltaTime * turningSpeed * sensitivity * Input.acceleration.x;
		
        modelSettings.RotateBasedOnMobileInput(modelSettings.model, 1);
        modelSettings.RotateBasedOnMobileInput(cameraUsed.transform, 1);

        hud.SetDistanceText(transform.position.z);

		KeyboardControl ();
	}

	void KeyboardControl ()
	{
        if (Input.GetKey(left))
			transform.position += transform.right * Time.deltaTime * sensitivity * -turningSpeed;
        else if (Input.GetKey(right))
			transform.position += transform.right * Time.deltaTime * sensitivity * turningSpeed;
	}

	void IncreaseSpeed ()
	{
        if (speed < 200)
        {
            speed += speedIncrease;
            hud.speedText.text = speed.ToString();
        }
	}

    public void StopMoving()
    {
        speed = 0f;    
        turningSpeed = 0f;
        CancelInvoke("IncreaseSpeed");
    }
}

[System.Serializable]
public class ModelSettings
{
    public float rotationSpeed;
    public float rotationLimit;
    public Transform model;
    float z = 0f;

    [HideInInspector]
    public Quaternion originalRotation;

    public void RotateBasedOnMobileInput(Transform t, int direction)
    {
        z = Input.acceleration.x * 15f;
        z = Mathf.Clamp(z, -rotationLimit, rotationLimit);

        t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z * direction);
    }
}
