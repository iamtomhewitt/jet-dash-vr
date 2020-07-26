using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace Player
{
	/// <summary>
	/// The HUD for the player.
	/// </summary>
	public class PlayerHud : MonoBehaviour
	{
		[SerializeField] private Text speedText;
		[SerializeField] private Text distanceText;
		[SerializeField] private Text scoreText;
		[SerializeField] private Text powerupNotificationText;
		[SerializeField] private Text relaunchingText;
		[SerializeField] private Image invincibilityBar;
		[SerializeField] private float notificationShowTime = 1.5f;

		private Animator notificationAnimator;

		private void Start()
		{
			notificationAnimator = powerupNotificationText.GetComponent<Animator>();
		}

		/// <summary>
		/// Formats the distance text, F0 puts a 0 in front of single digits.
		/// </summary>
		public void SetDistanceText(float distance)
		{
			distanceText.SetText(distance.ToString("F0"));
		}

		public void SetSpeedText(float speed)
		{
			speedText.SetText(speed.ToString());
		}

		public void SetScoreText(int score)
		{
			scoreText.SetText(score.ToString());
		}

		/// <summary>
		/// Shows a notification on the screen. Useful for showing speed streaks or power up messages.
		/// </summary>
		public void ShowNotification(Color32 color, string message)
		{
			StopAllCoroutines();
			powerupNotificationText.color = color;
			powerupNotificationText.SetText(message);
			notificationAnimator.Play(Constants.POWERUP_NOTIFY_SHOW);
			StartCoroutine(TurnOffNotification());
		}

		/// <summary>
		/// Hides a notification after a set amount of time.
		/// </summary>
		private IEnumerator TurnOffNotification()
		{
			yield return new WaitForSeconds(notificationShowTime);
			notificationAnimator.Play(Constants.POWERUP_NOTIFY_HIDE);
		}

		public Text GetRelaunchingText()
		{
			return relaunchingText;
		}

		public Text GetDistanceText()
		{
			return distanceText;
		}

		public Text GetSpeedText()
		{
			return speedText;
		}

		public Text GetScoreText()
		{
			return scoreText;
		}

		public Text GetPowerupNotificationText()
		{
			return powerupNotificationText;
		}
	}
}