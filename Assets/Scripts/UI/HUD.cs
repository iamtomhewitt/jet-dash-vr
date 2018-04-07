using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public Text speedText;
        public Text distanceText;
        public Text scoreText;
        public Text powerupNotificationText;
        public Image invincibilityBar;

        public void SetDistanceText(float distance)
        {
            distanceText.text = distance.ToString("F0");
        }

        public void SetScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        public void ShowPowerupNotification(Color32 color, string message)
        {
            StopAllCoroutines();
            powerupNotificationText.color = color;
            powerupNotificationText.text = message;
            powerupNotificationText.GetComponent<Animator>().Play("Powerup Notification Show");
            StartCoroutine(TurnOffNotification());
        }

        IEnumerator TurnOffNotification()
        {
            yield return new WaitForSeconds(1.5f);
            powerupNotificationText.GetComponent<Animator>().Play("Powerup Notification Hide");
        }
    }
}
