using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI;
using Spawner;
using Manager;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        public GameObject explosion;
        public GameObject gameHUD;
        public GameObject playerModel;

        public Scoreboard scoreboard;

		private void OnCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Obstacle":
                    if (PlayerPowerupManager.instance.godMode)
                        return;

                    GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                    Destroy(e, 3f);

                    scoreboard.Show();
                    scoreboard.AnimateDistanceScore(PlayerScore.instance.GetDistanceScore());
                    scoreboard.AnimateBonusScore(PlayerScore.instance.GetBonusScore());
                    scoreboard.AnimateTopSpeed(PlayerScore.instance.GetSpeed());
                    scoreboard.AnimateFinalScore(PlayerScore.instance.GetFinalScore());

					// Have to do this manually as having an instance of HighscoreManager causes problems with the way scores are displayed on the highscore scene
					PlayerScore.instance.SaveHighscore();

					PlayerControl.instance.StopMoving();
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
							PlayerPowerupManager.instance.BonusPoints(powerup);
                            break;

                        case PowerupType.DoublePoints:
							PlayerPowerupManager.instance.DoublePoints(powerup);
							break;

                        case PowerupType.Invincibility:
							PlayerPowerupManager.instance.Invincibility(powerup);
							break;
                    }

                    powerup.MovePosition();
                    break;
            }
        }		
	}
}