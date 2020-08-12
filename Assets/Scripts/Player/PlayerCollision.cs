using Achievements;
using LevelManagers;
using Manager;
using SpawnableObject;
using System.Collections;
using UnityEngine;
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
		private PlayerHud hud;
		private PlayerScore playerScore;
		private Scoreboard scoreboard;
		private ShopManager shopManager;
		private bool dead = false;

		private void Start()
		{
			achievementManager = AchievementManager.instance;
			audioManager = AudioManager.instance;
			highscoreManager = HighscoreManager.instance;
			hud = GetComponent<PlayerHud>();
			playerControl = GetComponent<PlayerControl>();
			playerScore = GetComponent<PlayerScore>();
			scoreboard = FindObjectOfType<Scoreboard>();
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

					dead = true;

					GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
					int topSpeed = playerControl.GetSpeed();

					Destroy(e, 3f);

					playerControl.StopMoving();
					playerModel.SetActive(false);
					gameHUD.SetActive(false);

					audioManager.Play(SoundNames.PLAYER_DEATH);
					audioManager.Pause(SoundNames.SHIP_ENGINE);

					shopManager.AddCash(playerScore.GetFinalScore());

					scoreboard.Show();
					scoreboard.AnimateDistanceScore(playerScore.GetDistanceScore());
					scoreboard.AnimateBonusScore(playerScore.GetBonusScore());
					scoreboard.AnimateTopSpeed(topSpeed);
					scoreboard.AnimateFinalScore(playerScore.GetFinalScore());

					bool newScore = highscoreManager.SaveLocalHighscore(playerScore.GetFinalScore());
					bool newDistance = highscoreManager.SaveDistanceHighscore(playerScore.GetDistanceScore());

					if (newScore || newDistance) 
					{
						hud.SetNewHighscoreText("New highscore!");
					}

					// Now update achievements
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_1000, 1000, playerScore.GetDistanceScore());
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_10000, 10000, playerScore.GetDistanceScore());
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_50000, 50000, playerScore.GetDistanceScore());
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_FIVE_MILLION, 5000000, playerScore.GetFinalScore());
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_HALF_MILLION, 500000, playerScore.GetFinalScore());
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_MILLION, 1000000, playerScore.GetFinalScore());
					achievementManager.UnlockAchievement(AchievementIds.DIE);

					StartCoroutine(RestartLevelRoutine());
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

		private IEnumerator RestartLevelRoutine()
		{
			yield return new WaitForSeconds(scoreboard.GetAnimationTime());

			for (int i = 3; i >= 0; i--)
			{
				yield return new WaitForSeconds(1);
				hud.GetRelaunchingText().SetText(Ui.RELAUNCHING(i));
			}

			hud.GetRelaunchingText().SetText(Ui.RELAUNCHING(-1));
			FindObjectOfType<GameLevelManager>().RestartLevel();
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

		public bool IsDead()
		{
			return dead;
		}

		public GameObject GetShield()
		{
			return shield;
		}
	}
}