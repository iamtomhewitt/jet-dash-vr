using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeHelper : MonoBehaviour
    {
        public float fadeSpeed = 0.75f;
        public CanvasGroup canvasGroup;
        public static FadeHelper instance;

        void Awake()
        {
            if (instance) 
            {
                DestroyImmediate (gameObject);
            } 
            else 
            {
                //DontDestroyOnLoad (this.gameObject);
                instance = this;
            }
        }   


        /// <summary>
        /// When enabled, subscribe to delegate.
        /// </summary>
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnNewLevelLoaded;
        }


        /// <summary>
        /// When disabled, unsubscribe to delegate.
        /// </summary>
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnNewLevelLoaded;
        }


        /// <summary>
        /// Whenever a new level/scene is loaded, it should be faded in.
        /// </summary>
        void OnNewLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            FadeIn();
        }


        /// <summary>
        /// Public method to call Fade Coroutine.
        /// </summary>
        public void FadeIn()
        {
            StartCoroutine(FadeInFromBlack());
        }

        /// <summary>
        /// Public method to call Fade Coroutine.
        /// </summary>
        public void FadeOut()
        {
            StartCoroutine(FadeOutToBlack());
        }


        /// <summary>
        /// Public method to call Fade Coroutine.
        /// </summary>
        public void FadeOutAndLoadScene(string sceneName)
        {
            StartCoroutine(FadeOutToBlackAndLoadScene(sceneName));
        }

        /// <summary>
        /// Turns the screen instant pitch black.
        /// </summary>
        public void BlackOut()
        {
            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// Coroutines for fading listed here.
        /// </summary>
        IEnumerator FadeInFromBlack()
        {
            canvasGroup.alpha = 1f;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }

            canvasGroup.interactable = false;
            yield return null;
        }


        IEnumerator FadeOutToBlack()
        {
            canvasGroup.alpha = 0f;

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }

            canvasGroup.interactable = false;
            yield return null;
        }


        IEnumerator FadeOutToBlackAndLoadScene(string sceneName)
        {
            canvasGroup.alpha = 0f;

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }

            canvasGroup.interactable = false;
            yield return null;
            SceneManager.LoadScene(sceneName);
        }
    }
}