using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class GameSettings : MonoBehaviour
    {
        private bool isVrMode;
        private float sensitivity;

        public static GameSettings instance;

		// TODO move to constants
		public const string highscoreKey = "LocalHighscore";
		public const string distanceKey = "DistanceHighscore";
		public const string uploadedKey = "hasUploadedHighscore";
		public const string privateCode = "VqnbsBo9LEe_iN-ksRCzyQ84P3n4pBLE6rPNBPAsjIpg";
		public const string publicCode = "5ac7c821d6024519e07786bd";
		public const string url = "http://dreamlo.com/lb/";

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
