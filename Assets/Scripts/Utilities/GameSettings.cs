using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class GameSettings : MonoBehaviour
    {
        public bool isVRMode;
        public float sensitivity;

        public static GameSettings instance;

        void Awake()
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

        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
