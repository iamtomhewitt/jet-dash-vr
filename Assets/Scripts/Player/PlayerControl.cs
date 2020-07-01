using Achievements;
using Data;
using Manager;
using UnityEngine;
using Utility;

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

		private AudioManager audioManager;
		private GameObject cameraToUse;
		private PlayerHud hud;
		private Transform shipModel;
		private bool reachedMaxSpeed = false;
		private float acceleration;
		private float deadzone = 0.035f;
		private float sensitivity;
		private float speed;
		private float startingSpeed = 20f;
		private float startingYPosition;
		private float turningSpeed;
		private float yPositionCheckRate = 3f;
		private float z = 0f;

		public void Start()
		{
			Initialise();

			InvokeRepeating("IncreaseSpeed", acceleration, acceleration);
			InvokeRepeating("CheckYPosition", yPositionCheckRate, yPositionCheckRate);

			audioManager = AudioManager.instance;
			audioManager.Play(SoundNames.SHIP_ENGINE);
			audioManager.Play(SoundNames.SHIP_STARTUP);
		}

		private void Initialise()
		{
			GameSettingsManager gs = GameSettingsManager.instance;
			normalCamera.SetActive(!gs.vrMode());
			VRCamera.SetActive(gs.vrMode());
			cameraToUse = gs.vrMode() ? VRCamera : normalCamera;
			sensitivity = gs.GetSensitivity();

			ShipData shipData = ShopManager.instance.GetSelectedShipData();
			shipModel = GameObject.FindGameObjectWithTag(shipData.GetShipName()).transform;
			speed = shipData.GetSpeed();
			acceleration = shipData.GetAcceleration();
			turningSpeed = shipData.GetTurningSpeed();

			foreach (Transform model in shipModels)
			{
				model.gameObject.SetActive(model.tag.Equals(shipData.GetShipName()));
			}

			hud = GetComponent<PlayerHud>();
			hud.SetSpeedText(speed.ToString());
			startingYPosition = transform.position.y;
		}

		private void Update()
		{
			// Move forward
			transform.position += transform.forward * Time.deltaTime * speed;

			// Move left and right based on accelerometer
			transform.position += transform.right * Time.deltaTime * turningSpeed * sensitivity * Input.acceleration.x;

			shipModel.RotateOnZAxisByAccelerometer(InputInDeadzone(), modelRotationLimit);
			cameraToUse.transform.RotateOnZAxisByAccelerometer(InputInDeadzone(), cameraRotationLimit);

			hud.SetDistanceText(transform.position.z);
		}

		private bool InputInDeadzone()
		{
			return Input.acceleration.x > -deadzone && Input.acceleration.x < deadzone;
		}

		public void IncreaseSpeed()
		{
			if (HasReachedMaxSpeed())
			{
				return;
			}
			
			if (speed == maxSpeed)
			{
				reachedMaxSpeed = true;
				AchievementManager.instance.UnlockAchievement(AchievementIds.GET_MAX_SPEED);
			}

			if (speed < maxSpeed)
			{
				speed += speedIncrease;
				hud.SetSpeedText(speed.ToString());

				float pitch = (speed / 1000f) + 1f;
				audioManager.GetSound(SoundNames.SHIP_ENGINE).pitch = pitch;
			}
		}

		public void CheckYPosition()
		{
			if (transform.position.y < (startingYPosition - 0.1f))
			{
				Debug.Log(string.Format("WARNING: Player Y position ({0}) has gone lower than expected ({1}), resetting", transform.position.y, startingYPosition));
				transform.SetYPosition(startingYPosition);
			}
		}

		public void StopMoving()
		{
			speed = 0f;
			turningSpeed = 0f;
			CancelInvoke("IncreaseSpeed");
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

		public float GetTurningSpeed()
		{
			return turningSpeed;
		}

		public bool HasReachedMaxSpeed()
		{
			return reachedMaxSpeed;
		}
	}
}