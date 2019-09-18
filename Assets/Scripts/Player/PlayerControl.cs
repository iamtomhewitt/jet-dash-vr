using UnityEngine;
using UI;
using Utilities;
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
		
		private GameObject cameraToUse;

        private float sensitivity;

		public static PlayerControl instance;

        [Space()]
        public ModelSettings modelSettings;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
        {
            PlayerHud.instance.speedText.text = speed.ToString();

            modelSettings.SelectRandomShip();
            modelSettings.originalRotation = modelSettings.model.rotation;

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
		
            modelSettings.RotateBasedOnMobileInput(modelSettings.model, modelRotationLimit);
            modelSettings.RotateBasedOnMobileInput(cameraToUse.transform, cameraRotationLimit);

            PlayerHud.instance.SetDistanceText(transform.position.z);

            KeyboardControl();
        }

        private void KeyboardControl()
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
            if (speed < 200)
            {
                speed += speedIncrease;
                PlayerHud.instance.speedText.text = speed.ToString();

                float p = (speed / 1000f) + 1f;
                AudioManager.instance.GetSound(SoundNames.SHIP_ENGINE).pitch = p;                
            }
        }

		private void CheckSpeedStreak()
		{
			if (speed % 50 == 0)
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
            else if (gs.isVRMode)
            {
                normalCamera.SetActive(false);
                VRCamera.SetActive(true);
                cameraToUse = VRCamera;
                sensitivity = gs.sensitivity;
            }
            else
            {
                normalCamera.SetActive(true);
                VRCamera.SetActive(false);
                cameraToUse = normalCamera;
                sensitivity = gs.sensitivity;
            }
        }

		/// <summary>
		/// A data class that holds player model information. It is also responsible for rotating the model based on mobile accelerometer,
		/// and for selecting a random ship at the start of the game.
		/// </summary>
        [System.Serializable]
        public class ModelSettings
        {
            public GameObject[] models;
            private float z = 0f;

            [HideInInspector]
            public Quaternion originalRotation;
            [HideInInspector]
            public Transform model;

			/// <summary>
			/// Rotates the ship based on the mobile accelerometer.
			/// </summary>
            public void RotateBasedOnMobileInput(Transform t, float limit)
            {
                z = Input.acceleration.x * 30f;
                z = Mathf.Clamp(z, -limit, limit);

                t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
            }

			/// <summary>
			/// Activates a random ship at the start of the game.
			/// </summary>
            public void SelectRandomShip()
            {
                for (int i = 0; i < models.Length; i++)
                {
                    models[i].SetActive(false);
                }

                int j = Random.Range(0, models.Length);
                model = models[j].transform;

                model.gameObject.SetActive(true);
            }
        }
    }
}


