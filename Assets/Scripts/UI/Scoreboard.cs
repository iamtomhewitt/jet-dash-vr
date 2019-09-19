using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace Player
{
	public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private Text distanceScoreText;
		[SerializeField] private Text bonusScoreText;
		[SerializeField] private Text topSpeed;
		[SerializeField] private Text finalScore;

		public static Scoreboard instance;

		private void Awake()
		{
			instance = this;
		}

		public void AnimateBonusScore(int score)
        {
            StartCoroutine(Animate(score, bonusScoreText, false));
        }

        public void AnimateDistanceScore(int score)
        {
            StartCoroutine(Animate(score, distanceScoreText, false));
        }

        public void AnimateTopSpeed(int score)
        {
            StartCoroutine(Animate(score, topSpeed, false));
        }

        public void AnimateFinalScore(int score)
        {
            StartCoroutine(Animate(score, finalScore, true));
        }

        private IEnumerator Animate(int score, Text text, bool isFinalScore)
        {
            int displayScore = 0;
            int start = displayScore;
            AudioManager.instance.Play("Score");

            for (float timer = 0; timer < 3f; timer += Time.deltaTime)
            {
                float progress = timer / 3f;
                displayScore = (int)Mathf.Lerp(start, score, progress);
                text.text = displayScore.ToString();
                yield return null;
            }

            displayScore = score;
            text.text = displayScore.ToString();
            AudioManager.instance.Pause("Score");

			// Final score will take the longest to animate, to only reload the scene when animating final score,
			// that way nothing gets cut off
			if (isFinalScore)
			{
				FindObjectOfType<GameLevelManager>().RestartLevel();
			}
        }

        public void Show()
        {
            StartCoroutine(ShowAndFadeOut());
        }

        private IEnumerator ShowAndFadeOut()
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;

            while (cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime * 1.5f;
                yield return null;
            }

            cg.interactable = false;
            yield return null;
        }
    }
}
