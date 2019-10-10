using UnityEngine;
using System.Collections;
using Spawner;
using Manager;
using Utility;
using Achievement;

namespace Player
{
	public class PlayerCollision : MonoBehaviour
	{
		[SerializeField] private GameObject explosion;
		[SerializeField] private GameObject gameHUD;
		[SerializeField] private GameObject playerModel;
		[SerializeField] private GameObject shield;

		[SerializeField] private bool godMode;

		private Coroutine godModeRoutine;

		private void OnCollisionEnter(Collision other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					if (godMode)
					{
						AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_OBSTACLE_WHEN_INVINCIBLE);
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

					// Now update achievements
					AchievementManager.instance.UnlockAchievement(AchievementIds.DIE);
					AchievementManager.instance.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_1000, 1000, PlayerScore.instance.GetDistanceScore());
					AchievementManager.instance.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_10000, 10000, PlayerScore.instance.GetDistanceScore());
					AchievementManager.instance.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_50000, 50000, PlayerScore.instance.GetDistanceScore());
					AchievementManager.instance.ProgressAchievement(AchievementIds.POINTS_OVER_HALF_MILLION, 500000, PlayerScore.instance.GetFinalScore());
					AchievementManager.instance.ProgressAchievement(AchievementIds.POINTS_OVER_MILLION, 1000000, PlayerScore.instance.GetFinalScore());
					AchievementManager.instance.ProgressAchievement(AchievementIds.POINTS_OVER_FIVE_MILLION, 5000000, PlayerScore.instance.GetFinalScore());
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
							AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_BONUS_POINTS);
							break;

						case PowerupType.DoublePoints:
							PlayerScore.instance.DoublePoints();
							AudioManager.instance.Play(SoundNames.DOUBLE_POINTS);
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "x2!");
							AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_DOUBLE_POINTS);
							break;

						case PowerupType.Invincibility:
							if (godModeRoutine != null)
							{
								StopCoroutine(godModeRoutine);
							}
							godModeRoutine = StartCoroutine(ActivateGodMode(5f));
							AudioManager.instance.Play(SoundNames.INVINCIBILITY_POINTS);
							PlayerHud.instance.ShowNotification(powerup.GetColour(), "Invincible!");
							AchievementManager.instance.UnlockAchievement(AchievementIds.BECOME_INVINCIBLE);
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