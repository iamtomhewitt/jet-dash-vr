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

		private float speed;
		private float speedIncreaseRate;
		private float turningSpeed;
		private float sensitivity;
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
			PlayerHud.instance.SetSpeedText(speed.ToString());
			startingYPosition = transform.position.y;

			ApplyShipSettings();
			ApplyCameraSettings();

			InvokeRepeating("IncreaseSpeed", speedIncreaseRate, speedIncreaseRate);
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

			RotateBasedOnMobileInput(shipModel, modelRotationLimit);
			RotateBasedOnMobileInput(cameraToUse.transform, cameraRotationLimit);

			PlayerHud.instance.SetDistanceText(transform.position.z);
		}

		/// <summary>
		/// Rotates the ship based on the mobile accelerometer.
		/// </summary>
		private void RotateBasedOnMobileInput(Transform t, float limit)
		{
			z = Input.acceleration.x * 30f;
			z = Mathf.Clamp(z, -limit, limit);

			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
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
				Debug.Log(string.Format("WARNING: Player Y position ({0}) has gone lower than expected ({1}), resetting.", transform.position.y, startingYPosition));
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
			InvokeRepeating("IncreaseSpeed", speedIncreaseRate, speedIncreaseRate);
		}

		private void ApplyCameraSettings()
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
		}

		private void ApplyShipSettings()
		{
			ShipData shipData	= ShopManager.instance.GetSelectedShipData();
			shipModel			= GameObject.FindGameObjectWithTag(shipData.GetShipName()).transform;
			originalRotation	= shipModel.rotation;
			speed				= shipData.GetSpeed();
			speedIncreaseRate	= shipData.GetAcceleration();
			turningSpeed		= shipData.GetTurningSpeed();

			foreach (Transform model in shipModels)
			{
				model.gameObject.SetActive((model.tag.Equals(shipData.GetShipName())));
			}
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

		public float GetSpeedIncreaseRate()
		{
			return speedIncreaseRate;
		}

		public bool HasReachedMaxSpeed()
		{
			return reachedMaxSpeed;
		}
	}
}
