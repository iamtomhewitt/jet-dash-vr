using UnityEngine;
using Achievements;
using Data;
using Manager;

namespace Player
{
	public class PlayerControl : MonoBehaviour
	{
		[SerializeField] private GameObject normalCamera;
		[SerializeField] private GameObject VRCamera;
		[SerializeField] private Transform[] shipModels;

		[SerializeField] private float speedIncrease;
		[SerializeField] private float modelRotationLimit;
		[SerializeField] private float cameraRotationLimit;
		[SerializeField] private float maxSpeed = 200f;

		private GameObject cameraToUse;
		private Transform shipModel;
		private Quaternion originalRotation;
		private PlayerHud hud;

		private float speed;
		private float acceleration;
		private float turningSpeed;
		private float sensitivity;
		private float deadzone = 0.05f;
		private float z = 0f;
		private float yPositionCheckRate = 3f;
		private float startingYPosition;
		private bool reachedMaxSpeed = false;

		public static PlayerControl instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			Initialise();

			InvokeRepeating("IncreaseSpeed", acceleration, acceleration);
			InvokeRepeating("CheckYPosition", yPositionCheckRate, yPositionCheckRate);
			
			AudioManager.instance.Play(SoundNames.SHIP_ENGINE);
			AudioManager.instance.Play(SoundNames.SHIP_STARTUP);
		}

		private void Update()
		{
			// Move forward
			transform.position += transform.forward * Time.deltaTime * speed;

			// Move left and right based on accelerometer
			transform.position += transform.right * Time.deltaTime * turningSpeed * sensitivity * Input.acceleration.x;

			RotateUsingAccelerometer(shipModel, modelRotationLimit);
			RotateUsingAccelerometer(cameraToUse.transform, cameraRotationLimit);

			hud.SetDistanceText(transform.position.z);
		}

		private void Initialise()
		{
			GameSettingsManager gs = GameSettingsManager.instance;
			if (gs == null)
			{
				normalCamera.SetActive(true);
				VRCamera.SetActive(false);
				cameraToUse = normalCamera;
				sensitivity = 1f;
			}
			else if (gs.vrMode())
			{
				normalCamera.SetActive(false);
				VRCamera.SetActive(true);
				cameraToUse = VRCamera;
				sensitivity = gs.GetSensitivity();
			}
			else
			{
				normalCamera.SetActive(true);
				VRCamera.SetActive(false);
				cameraToUse = normalCamera;
				sensitivity = gs.GetSensitivity();
			}

			ShipData shipData	= ShopManager.instance.GetSelectedShipData();
			shipModel			= GameObject.FindGameObjectWithTag(shipData.GetShipName()).transform;
			originalRotation	= shipModel.rotation;
			speed				= shipData.GetSpeed();
			acceleration		= shipData.GetAcceleration();
			turningSpeed		= shipData.GetTurningSpeed();

			foreach (Transform model in shipModels)
			{
				model.gameObject.SetActive((model.tag.Equals(shipData.GetShipName())));
			}

			hud = PlayerHud.instance;
			hud.SetSpeedText(speed.ToString());
			startingYPosition = transform.position.y;
		}

		public void OnGUI()
		{
			GUI.Label(new Rect(10, 10, 200, 100), "Deadzone: " + deadzone);
		}

		private void RotateUsingAccelerometer(Transform t, float limit)
		{
			if (!InputInDeadzone())
			{
				z = Input.acceleration.x * 30f;
				z = Mathf.Clamp(z, -limit, limit);

				t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
			}
		}

		private bool InputInDeadzone()
		{
			return Input.acceleration.x > -deadzone && Input.acceleration.x < deadzone;
		}

		private void IncreaseSpeed()
		{
			if (speed == maxSpeed)
			{
				reachedMaxSpeed = true;
				AchievementManager.instance.UnlockAchievement(AchievementIds.GET_MAX_SPEED);
			}

			if (speed < maxSpeed)
			{
				speed += speedIncrease;
				PlayerHud.instance.SetSpeedText(speed.ToString());

				float p = (speed / 1000f) + 1f;
				AudioManager.instance.GetSound(SoundNames.SHIP_ENGINE).pitch = p;
			}
		}

		private void CheckYPosition()
		{
			if (transform.position.y < (startingYPosition - 0.1f))
			{
				Debug.Log(string.Format("WARNING: Player Y position ({0}) has gone lower than expected ({1}), resetting", transform.position.y, startingYPosition));
				transform.position = new Vector3(transform.position.x, startingYPosition, transform.position.z);
			}
		}

		public void StopMoving()
		{
			speed = 0f;
			turningSpeed = 0f;
			CancelInvoke("IncreaseSpeed");
		}

		public void StartMoving()
		{
			speed = 20f;
			turningSpeed = 20f;
			InvokeRepeating("IncreaseSpeed", acceleration, acceleration);
		}

		public int GetSpeed()
		{
			return (int)speed;
		}

		public void SetSpeed(float speed)
		{
			this.speed = speed;
		}

		public void MaxSpeed()
		{
			speed = maxSpeed;
		}

		public float GetAcceleration()
		{
			return acceleration;
		}

		public bool HasReachedMaxSpeed()
		{
			return reachedMaxSpeed;
		}
	}
}
