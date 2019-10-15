using UnityEngine;

namespace Utility
{
	public class GameSettings : MonoBehaviour
    {
        private bool isVrMode;
        private float sensitivity;

        public static GameSettings instance;		

		private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
        }

        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

		public bool vrMode()
		{
			return isVrMode;
		}

		public void SetVrMode(bool vrMode)
		{
			isVrMode = vrMode;
		}

		public float GetSensitivity()
		{
			return sensitivity;
		}

		public void SetSensitivity(float s)
		{
			sensitivity = s;
		}
    }
}
