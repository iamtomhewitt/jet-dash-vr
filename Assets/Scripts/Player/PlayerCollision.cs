using UnityEngine;
using UI;
using Spawner;
using Manager;
using System.Collections;

namespace Player
{
	public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private GameObject explosion;
		[SerializeField] private GameObject gameHUD;
		[SerializeField] private GameObject playerModel;
		[SerializeField] private GameObject shield;

		[SerializeField] private bool godMode;

		private Coroutine godModRoutine;

		private void OnCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Obstacle":
					if (godMode)
					{
						return;
					}

                    GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                    Destroy(e, 3f);

                    Scoreboard.instance.Show();
					Scoreboard.instance.AnimateDistanceScore(PlayerScore.instance.GetDistanceScore());
					Scoreboard.instance.AnimateBonusScore(PlayerScore.instance.GetBonusScore());
					Scoreboard.instance.AnimateTopSpeed(PlayerControl.instance.GetSpeed());
					Scoreboard.instance.AnimateFinalScore(PlayerScore.instance.GetFinalScore());

					// Have to do this manually as having an instance of HighscoreManager causes problems with the way scores are displayed on the highscore scene
					// TODO FIX THIS - THESE SHOULD BE PART OF THE HIGHSCORE MANAGER INSTANCE
					PlayerScore.instance.SaveHighscore();
					PlayerScore.instance.SaveDistanceHighscore();

					PlayerControl.instance.StopMoving();
                    playerModel.SetActive(false);
                    gameHUD.SetActive(false);

                    AudioManager.instance.Play(SoundNames.PLAYER_DEATH);
                    AudioManager.instance.Pause(SoundNames.SHIP_ENGINE);
                    break;

				default:
					// Nothing to do!
					break;
            }
        }

		private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Powerup":
                    Powerup powerup = other.GetComponent<Powerup>();

					switch (powerup.GetPowerupType())
					{
						case PowerupType.BonusPoints:
							PlayerScore.instance.AddBonusPoints(500);
							AudioManager.instance.Play("Powerup Bonus Points");
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "+500!");
							break;

						case PowerupType.DoublePoints:
							PlayerScore.instance.DoublePoints();
							AudioManager.instance.Play("Powerup Double Points");
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "x2!");
							break;

						case PowerupType.Invincibility:
							StopCoroutine(godModRoutine);
							godModRoutine = StartCoroutine(ActivateGodMode(5f));
							AudioManager.instance.Play("Powerup Invincibility");
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "Invincible!");
							break;

						default:
							Debug.Log("Warning! Unrecognised Powerup type: " + powerup.GetPowerupType());
							break;
					}

					powerup.MovePosition();
                    break;

				default:
					// Nothing to do!
					break;
			}
		}

		/// <summary>
		/// Makes the player invincible for a set amount of time, and shows the shield.
		/// </summary>
		private IEnumerator ActivateGodMode(float duration)
		{
			godMode = true;

			shield.SetActive(true);
			shield.GetComponent<Animator>().Play("On");

			yield return new WaitForSeconds(duration);

			// Now flicker
			for (int i = 0; i < 3; i++)
			{
				shield.SetActive(false);
				yield return new WaitForSeconds(.3f);
				shield.SetActive(true);
				yield return new WaitForSeconds(.3f);
			}

			shield.SetActive(false);

			godMode = false;
		}
	}
}