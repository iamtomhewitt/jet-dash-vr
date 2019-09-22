using UnityEngine;
using Spawner;
using Manager;
using System.Collections;
using Utility;

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
                case Tags.OBSTACLE:
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

					HighscoreManager.instance.SaveLocalHighscore(PlayerScore.instance.GetFinalScore());
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
                case Tags.POWERUP:
                    Powerup powerup = other.GetComponent<Powerup>();

					switch (powerup.GetPowerupType())
					{
						case PowerupType.BonusPoints:
							PlayerScore.instance.AddBonusPoints(500);
							AudioManager.instance.Play(SoundNames.BONUS_POINTS);
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "+500!");
							break;

						case PowerupType.DoublePoints:
							PlayerScore.instance.DoublePoints();
							AudioManager.instance.Play(SoundNames.DOUBLE_POINTS);
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "x2!");
							break;

						case PowerupType.Invincibility:
							StopCoroutine(godModRoutine);
							godModRoutine = StartCoroutine(ActivateGodMode(5f));
							AudioManager.instance.Play(SoundNames.INVINCIBILITY_POINTS);
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "Invincible!");
							break;

						default:
							Debug.Log("Warning! Unrecognised Powerup type: " + powerup.GetPowerupType());
							break;
					}

					powerup.Relocate();
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