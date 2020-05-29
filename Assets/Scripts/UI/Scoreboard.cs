using LevelManagers;
using Manager;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
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

		private const float ANIMATION_TIME = 3f;

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
			AudioManager audioManager = AudioManager.instance;
			int displayScore = 0;
			int start = displayScore;

			audioManager.Play(SoundNames.SCORE);

			for (float timer = 0; timer < ANIMATION_TIME; timer += Time.deltaTime)
			{
				float progress = timer / ANIMATION_TIME;
				displayScore = (int)Mathf.Lerp(start, score, progress);
				text.SetText( displayScore.ToString());
				yield return null;
			}

			displayScore = score;
			text.SetText( displayScore.ToString());
			audioManager.Pause(SoundNames.SCORE);

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
				relaunchingText.SetText(Ui.RELAUNCHING(i));
			}

			relaunchingText.SetText(Ui.RELAUNCHING(-1));
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
