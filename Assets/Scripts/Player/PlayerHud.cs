using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
		[SerializeField] private Image invincibilityBar;
		[SerializeField] private float notificationShowTime = 1.5f;

		private Animator notificationAnimator;

		public static PlayerHud instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			notificationAnimator = powerupNotificationText.GetComponent<Animator>();
		}

		/// <summary>
		/// Formats the distance text, F0 puts a 0 in front of single digits.
		/// </summary>
		public void SetDistanceText(float distance)
        {
            distanceText.text = distance.ToString("F0");
        }

		public void SetSpeedText(string message)
		{
			speedText.text = message;
		}

        public void SetScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

		/// <summary>
		/// Shows a notification on the screen. Useful for showing speed streaks or power up messages.
		/// </summary>
        public void ShowNotification(Color32 color, string message)
        {
            StopAllCoroutines();
            powerupNotificationText.color = color;
            powerupNotificationText.text = message;
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
    }
}
