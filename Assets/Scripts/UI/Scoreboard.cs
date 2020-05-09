using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Manager;
using LevelManagers;
using Utility;

namespace Player
{
	public class Scoreboard : MonoBehaviour
	{
		[SerializeField] private Text distanceScoreText;
		[SerializeField] private Text bonusScoreText;
		[SerializeField] private Text topSpeed;
		[SerializeField] private Text finalScore;
		[SerializeField] private Text relaunchingText;

		public static Scoreboard instance;

		private void Awake()
		{
			instance = this;
		}

		public void AnimateBonusScore(int score)
		{
			StartCoroutine(Animate(score, bonusScoreText));
		}

		public void AnimateDistanceScore(int score)
		{
			StartCoroutine(Animate(score, distanceScoreText));
		}

		public void AnimateTopSpeed(int score)
		{
			StartCoroutine(Animate(score, topSpeed));
		}

		public void AnimateFinalScore(int score)
		{
			if (ShopManager.instance.GetSelectedShipData().GetShipName().Equals(Tags.CELLEX))
			{
				finalScore.color = Color.yellow;
			}

			StartCoroutine(Animate(score, finalScore));
		}

		private IEnumerator Animate(int score, Text text)
		{
			int displayScore = 0;
			int start = displayScore;
			AudioManager.instance.Play(SoundNames.SCORE);

			for (float timer = 0; timer < 3f; timer += Time.deltaTime)
			{
				float progress = timer / 3f;
				displayScore = (int)Mathf.Lerp(start, score, progress);
				text.text = displayScore.ToString();
				yield return null;
			}

			displayScore = score;
			text.text = displayScore.ToString();
			AudioManager.instance.Pause(SoundNames.SCORE);

			// Final score will take the longest to animate, so only reload the scene when animating final score,
			// that way nothing gets cut off
			if (text.Equals(finalScore))
			{
				StartCoroutine(WaitAndRestart());
			}
		}

		private IEnumerator WaitAndRestart()
		{
			for (int i = 3; i >= 0; i--)
			{
				yield return new WaitForSeconds(1);
				relaunchingText.text = "Relaunching in: " + i.ToString();
			}

			relaunchingText.text = "Relaunching";
			FindObjectOfType<GameLevelManager>().RestartLevel();
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
