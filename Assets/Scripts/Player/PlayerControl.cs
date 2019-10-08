using UnityEngine;
using Utility;
using Manager;

namespace Player
{
	public class PlayerControl : MonoBehaviour
    {
		[SerializeField] private GameObject normalCamera;
		[SerializeField] private GameObject VRCamera;

		[SerializeField] private KeyCode left;
		[SerializeField] private KeyCode right;

		[SerializeField] private float speed;
		[SerializeField] private float speedIncrease;
		[SerializeField] private float speedIncreaseRepeatRate;
		[SerializeField] private float turningSpeed;
		[SerializeField] private float modelRotationLimit;
		[SerializeField] private float cameraRotationLimit;
		[SerializeField] private float maxSpeed = 200f;

		private GameObject cameraToUse;
		private PlayerModelSettings modelSettings;

		private float sensitivity;
		private bool reachedMaxSpeed = false;

		public static PlayerControl instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
        {
            PlayerHud.instance.SetSpeedText(speed.ToString());

			modelSettings = GetComponent<PlayerModelSettings>();
            modelSettings.SelectRandomShip();
			modelSettings.SetOriginalRotation();

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
		
            modelSettings.RotateBasedOnMobileInput(modelSettings.GetModel(), modelRotationLimit);
            modelSettings.RotateBasedOnMobileInput(cameraToUse.transform, cameraRotationLimit);

            PlayerHud.instance.SetDistanceText(transform.position.z);

            UseKeyboard();
        }

        private void UseKeyboard()
        {
			if (Input.GetKey(left))
			{
				transform.position += transform.right * Time.deltaTime * sensitivity * -turningSpeed;
			}

			else if (Input.GetKey(right))
			{
				transform.position += transform.right * Time.deltaTime * sensitivity * turningSpeed;
			}
        }

		public int GetSpeed()
		{
			return (int)speed;
		}

        private void IncreaseSpeed()
		{
			if (speed == maxSpeed)
			{
				reachedMaxSpeed = true;
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
            GameSettings gs = GameSettings.instance;

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
    }
}


