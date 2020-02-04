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
		[SerializeField] private PlayerShip ship;

		[SerializeField] private float speed;
		[SerializeField] private float speedIncrease;
		[SerializeField] private float speedIncreaseRepeatRate;
		[SerializeField] private float turningSpeed;
		[SerializeField] private float modelRotationLimit;
		[SerializeField] private float cameraRotationLimit;
		[SerializeField] private float maxSpeed = 200f;

		private GameObject cameraToUse;
		private Transform shipModel;
		private Quaternion originalRotation;

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

			ShowShipModel();
			originalRotation = shipModel.rotation;

			InvokeRepeating("IncreaseSpeed", speedIncreaseRepeatRate, speedIncreaseRepeatRate);
			InvokeRepeating("CheckSpeedStreak", speedIncreaseRepeatRate, speedIncreaseRepeatRate);

			DetermineGameSettings();

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

		private void CheckSpeedStreak()
		{
			if (speed % 50 == 0 && !reachedMaxSpeed)
			{
				PlayerHud.instance.ShowNotification(Color.white, speed + " Speed Streak!");
				AudioManager.instance.Play(SoundNames.SPEED_STREAK);
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
			InvokeRepeating("IncreaseSpeed", speedIncreaseRepeatRate, speedIncreaseRepeatRate);
		}

		/// <summary>
		/// Sets up the cameras and the sensitivity based upon what was selected in the main menu.
		/// </summary>
		private void DetermineGameSettings()
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

		private void ShowShipModel()
		{
			shipModel = GameObject.FindGameObjectWithTag(ship.GetShipName()).transform;

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
	}
}
