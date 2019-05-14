using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI;
using Spawner;
using Manager;
using Utilities;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        public GameObject explosion;
        public GameObject gameHUD;
        public GameObject playerModel;

        public Scoreboard scoreboard;

        private HUD hud;

        private PlayerControl playerControl;
        private PlayerScore playerScore;

        public bool godMode;

		private void Start()
        {
            playerControl = GetComponent<PlayerControl>();
            playerScore = GetComponent<PlayerScore>();
            hud = GetComponent<HUD>();
        }

		private void OnCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Obstacle":
                    if (godMode)
                        return;

                    GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                    Destroy(e, 3f);

                    scoreboard.Show();
                    scoreboard.AnimateDistanceScore(playerScore.GetDistanceScore());
                    scoreboard.AnimateBonusScore(playerScore.GetBonusScore());
                    scoreboard.AnimateTopSpeed(playerScore.GetSpeed());
                    scoreboard.AnimateFinalScore(playerScore.GetFinalScore());

					// Have to do this manually as having an instance of HighscoreManager causes problems with the way scores are displayed on the highscore scene
					SaveHighscore();

					playerControl.StopMoving();
                    playerModel.SetActive(false);
                    gameHUD.SetActive(false);

                    AudioManager.instance.Play("Player Death");
                    AudioManager.instance.Pause("Ship Hum");
                    break;
            }
        }

		private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Powerup":
                    Powerup powerup = other.GetComponent<Powerup>();

                    switch (powerup.powerupType)
                    {
                        case PowerupType.BonusPoints:
                            playerScore.AddBonusPoints(500);
                            AudioManager.instance.Play("Powerup Bonus Points");
                            hud.ShowNotification(powerup.color, "+500!");
                            break;

                        case PowerupType.DoublePoints:
                            playerScore.DoublePoints();
                            AudioManager.instance.Play("Powerup Double Points");
                            hud.ShowNotification(powerup.color, "x2!");
                            break;

                        case PowerupType.Invincibility:
                            StopAllCoroutines();
                            StartCoroutine(GodMode(5f));
                            AudioManager.instance.Play("Powerup Invincibility");
                            hud.ShowNotification(powerup.color, "Invincible!");
                            break;
                    }

                    powerup.MovePosition();
                    break;
            }
        }


		/// <summary>
		/// Makes the player invincible for a set amount of time. Also shows the invincibility bar.
		/// </summary>
		private IEnumerator GodMode(float duration)
        {
            godMode = true;
            Vector3 originalScale = new Vector3(15f, 1f, 1f);
            Vector3 targetScale = new Vector3(0f, 1f, 1f);

            float timer = 0f;

            do
            {
                hud.invincibilityBar.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            while (timer <= duration);

            hud.invincibilityBar.transform.localScale = Vector3.zero;
            godMode = false;
        }


		/// <summary>
		/// Saves the players score to the PlayerPrefs.
		/// </summary>
		private void SaveHighscore()
		{
			int currentHighscore = PlayerPrefs.GetInt(GameSettings.playerPrefsKey);
			int score = playerScore.GetFinalScore();
			if (score > currentHighscore)
			{
				print("New highscore of " + score + "! Saving...");
				PlayerPrefs.SetInt(GameSettings.playerPrefsKey, score);

				// Player has got a new highscore, which obvs hasnt been uploaded yet
				PlayerPrefs.SetInt(GameSettings.uploadedKey, 0);
			}
		}
	}
}