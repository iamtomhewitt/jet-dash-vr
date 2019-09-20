using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

		public static PlayerHud instance;

		private void Awake()
		{
			instance = this;
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
            powerupNotificationText.GetComponent<Animator>().Play("Powerup Notification Show");
            StartCoroutine(TurnOffNotification());
        }


		/// <summary>
		/// Hides a notification after a set amount of time.
		/// </summary>
        private IEnumerator TurnOffNotification()
        {
            yield return new WaitForSeconds(1.5f);
            powerupNotificationText.GetComponent<Animator>().Play("Powerup Notification Hide");
        }
    }
}
