using UnityEngine;
using System.Collections;
using SpawnableObject;
using Manager;
using Utility;
using Achievements;

namespace Player
{
	public class PlayerCollision : MonoBehaviour
	{
		[SerializeField] private GameObject explosion;
		[SerializeField] private GameObject gameHUD;
		[SerializeField] private GameObject playerModel;
		[SerializeField] private GameObject shield;

		[SerializeField] private bool godMode;

		public static PlayerCollision instance;

		private void Awake()
		{
			instance = this;
		}

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

					ShopManager.instance.AddCash(PlayerScore.instance.GetFinalScore());

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
					powerup.ApplyPowerupEffect();
					powerup.Relocate();
					break;

				default:
					// Nothing to do!
					break;
			}
		}

		public void SetGodMode(bool godMode)
		{
			this.godMode = godMode;
		}

		public GameObject GetShield()
		{
			return shield;
		}
	}
}