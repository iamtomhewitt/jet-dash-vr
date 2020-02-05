using UnityEngine;
using Manager;
using Achievements;

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
		private bool reachedMaxSpeed = false;

		public static PlayerControl instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			PlayerHud.instance.SetSpeedText(speed.ToString());

			ApplyShipSettings();
			ApplyCameraSettings();

			InvokeRepeating("IncreaseSpeed", speedIncreaseRate, speedIncreaseRate);
			
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

		/// <summary>
		/// Sets up the cameras based upon what was selected in the main menu.
		/// </summary>
		private void ApplyCameraSettings()
		{
			// TODO: sensitivity may need removing as these will be based on ship (either that, or combine them together)
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
			PlayerShip ship = ShopManager.instance.GetSelectedShip();
			shipModel			= GameObject.FindGameObjectWithTag(ship.GetShipName()).transform;
			speed				= ship.GetSpeed();
			speedIncreaseRate	= ship.GetSpeedIncreaseRate();
			turningSpeed		= ship.GetTurningSpeed();
			originalRotation	= shipModel.rotation;

			foreach (Transform model in shipModels)
			{
				if (model.tag.Equals(ship.GetShipName()))
				{
					model.gameObject.SetActive(true);
				}
				else
				{
					model.gameObject.SetActive(false);
				}
			}
		}

		public int GetSpeed()
		{
			return (int)speed;
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
