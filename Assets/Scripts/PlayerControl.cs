﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public float speed;
	public float speedIncrease;
	public float speedIncreaseRepeatRate;
	public float turningSpeed;

	private HUD hud;
    public GameObject cam;

	public KeyCode left;
	public KeyCode right;

    [Space()]
    public ModelSettings modelSettings;

	void Start ()
	{
		hud = GetComponent<HUD> ();
		hud.speedText.text = speed.ToString ();

        modelSettings.originalRotation = modelSettings.model.rotation;

		InvokeRepeating ("IncreaseSpeed", speedIncreaseRepeatRate, speedIncreaseRepeatRate);
	}

	void Update ()
	{
        // Move forward
		transform.position += transform.forward * Time.deltaTime * speed;

        // Move left and right based on accelerometer
		transform.position += transform.right * Time.deltaTime * turningSpeed * Input.acceleration.x;
		
        modelSettings.RotateBasedOnMobileInput(modelSettings.model, 1);
        modelSettings.RotateBasedOnMobileInput(cam.transform, 1);

        hud.distanceText.text = transform.position.z.ToString ("F0");

		KeyboardControl ();
	}

	void KeyboardControl ()
	{
        if (Input.GetKey(left))
            transform.position += transform.right * Time.deltaTime * -turningSpeed;
        else if (Input.GetKey(right))
            transform.position += transform.right * Time.deltaTime * turningSpeed;
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
        print("Todo, explode");
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        print("todo, advert after so many restarts");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
