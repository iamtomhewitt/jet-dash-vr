using UnityEngine;
using Achievements;
using Manager;
using SpawnableObject;
using Utility;

namespace Player
{
	public class PlayerCollision : MonoBehaviour
	{
		[SerializeField] private GameObject explosion;
		[SerializeField] private GameObject gameHUD;
		[SerializeField] private GameObject playerModel;
		[SerializeField] private GameObject shield;

		[SerializeField] private bool invincible;
		[SerializeField] private bool hyperdriveEnabled;

		private AchievementManager achievementManager;
		private AudioManager audioManager;
		private HighscoreManager highscoreManager;
		private PlayerControl playerControl;
		private PlayerScore playerScore;
		private Scoreboard scoreboard;
		private ShopManager shopManager;

		public static PlayerCollision instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			achievementManager = AchievementManager.instance;
			audioManager = AudioManager.instance;
			highscoreManager = HighscoreManager.instance;
			playerControl = PlayerControl.instance;
			playerScore = PlayerScore.instance;
			scoreboard = Scoreboard.instance;
			shopManager = ShopManager.instance;
		}

		private void OnCollisionEnter(Collision other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					if (IsGodMode())
					{
						achievementManager.UnlockAchievement(AchievementIds.FLY_THROUGH_OBSTACLE_WHEN_INVINCIBLE);
						return;
					}

					GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
					Destroy(e, 3f);

					scoreboard.Show();
					scoreboard.AnimateDistanceScore(playerScore.GetDistanceScore());
					scoreboard.AnimateBonusScore(playerScore.GetBonusScore());
					scoreboard.AnimateTopSpeed(playerControl.GetSpeed());
					scoreboard.AnimateFinalScore(playerScore.GetFinalScore());

					highscoreManager.SaveLocalHighscore(playerScore.GetFinalScore());
					playerScore.SaveDistanceHighscore();

					playerControl.StopMoving();
					playerModel.SetActive(false);
					gameHUD.SetActive(false);

					audioManager.Play(SoundNames.PLAYER_DEATH);
					audioManager.Pause(SoundNames.SHIP_ENGINE);

					shopManager.AddCash(playerScore.GetFinalScore());

					// Now update achievements
					achievementManager.UnlockAchievement(AchievementIds.DIE);
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_1000, 1000, playerScore.GetDistanceScore());
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_10000, 10000, playerScore.GetDistanceScore());
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_50000, 50000, playerScore.GetDistanceScore());
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_HALF_MILLION, 500000, playerScore.GetFinalScore());
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_MILLION, 1000000, playerScore.GetFinalScore());
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_FIVE_MILLION, 5000000, playerScore.GetFinalScore());
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

		public void SetInvincible(bool invincible)
		{
			this.invincible = invincible;
		}

		public void SetHyperdriveEnabled(bool enabled)
		{
			this.hyperdriveEnabled = enabled;
		}

		public bool IsGodMode()
		{
			return invincible || hyperdriveEnabled;
		}

		public GameObject GetShield()
		{
			return shield;
		}
	}
}