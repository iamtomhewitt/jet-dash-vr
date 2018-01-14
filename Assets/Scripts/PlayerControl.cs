using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public float speed;
	public float speedIncrease;
	public float speedIncreaseRepeatRate;
	public float turningSpeed;

	private HUD hud;

	#if UNITY_EDITOR
	public KeyCode left;
	public KeyCode right;
	#endif

	void Start ()
	{
		hud = GetComponent<HUD> ();
		hud.speedText.text = speed.ToString ();

		InvokeRepeating ("IncreaseSpeed", speedIncreaseRepeatRate, speedIncreaseRepeatRate);
	}

	void Update ()
	{
		transform.position += transform.forward * Time.deltaTime * speed;
		transform.position += transform.right * Time.deltaTime * turningSpeed * Input.acceleration.x;
		
        // Todo rotate model based on phone

        hud.distanceText.text = transform.position.z.ToString ("F0");

		#if UNITY_EDITOR
		KeyboardControl ();
		#endif
	}

	void KeyboardControl ()
	{
		if (Input.GetKey (left))
			transform.position += transform.right * Time.deltaTime * -turningSpeed;
		if (Input.GetKey (right))
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
}
