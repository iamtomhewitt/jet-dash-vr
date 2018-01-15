using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public float speed;
	public float speedIncrease;
	public float speedIncreaseRepeatRate;
	public float turningSpeed;

    [Space()]
    public ModelSettings modelSettings;

	private HUD hud;

    [Space()]
	public KeyCode left;
	public KeyCode right;

	void Start ()
	{
		hud = GetComponent<HUD> ();
		hud.speedText.text = speed.ToString ();
        modelSettings.originalRotation = modelSettings.model.rotation;

		InvokeRepeating ("IncreaseSpeed", speedIncreaseRepeatRate, speedIncreaseRepeatRate);
	}

	void Update ()
	{
		transform.position += transform.forward * Time.deltaTime * speed;
		transform.position += transform.right * Time.deltaTime * turningSpeed * Input.acceleration.x;
		
        modelSettings.RotateBasedOnMobileInput();

        hud.distanceText.text = transform.position.z.ToString ("F0");

		KeyboardControl ();
	}

	void KeyboardControl ()
	{
        if (Input.GetKey(left))
        {
            transform.position += transform.right * Time.deltaTime * -turningSpeed;
            modelSettings.RotateBasedOnKeyboardInput(modelSettings.rotationLimit);
        }
        else if (Input.GetKey(right))
        {
            transform.position += transform.right * Time.deltaTime * turningSpeed;
            modelSettings.RotateBasedOnKeyboardInput(-modelSettings.rotationLimit);
        }
        else
        {
            //modelSettings.RotateBackToOriginalRotation();
        }            
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

    public void RotateBasedOnMobileInput()
    {
//        if (Input.acceleration.x < -.5f)
//            model.rotation = Quaternion.Slerp(model.rotation, Quaternion.Euler(0f, 0f, rotationLimit), rotationSpeed * Time.deltaTime);
//        if (Input.acceleration.x > .5f)
//            model.rotation = Quaternion.Slerp(model.rotation, Quaternion.Euler(0f, 0f, -rotationLimit), rotationSpeed * Time.deltaTime);
        z = Input.acceleration.x * 50f;
        z = Mathf.Clamp(z, -45f, 45f);

        model.localEulerAngles = new Vector3(model.localEulerAngles.x, model.localEulerAngles.y, -z);
    }

    public void RotateBasedOnKeyboardInput(float limit)
    {
        model.rotation = Quaternion.Slerp(model.rotation, Quaternion.Euler(0f, 0f, limit), rotationSpeed * Time.deltaTime);
    }

    public void RotateBackToOriginalRotation()
    {
        model.rotation = Quaternion.Slerp(model.rotation,  originalRotation, rotationSpeed * Time.deltaTime);
    }
}
