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

					GameObject e = Instantiate(explosion) as GameObject;
					e.transform.SetPosition(new Vector3(transform.position.x, 0.6f, transform.position.z));
					
					int topSpeed = playerControl.GetSpeed();
					int finalScore = playerScore.GetFinalScore();
					int bonusScore = playerScore.GetBonusScore();
					int distanceScore = playerScore.GetDistanceScore();

					Destroy(e, 3f);

					playerControl.StopMoving();
					playerModel.SetActive(false);
					hud.SetNewHighscoreText("");
					gameHUD.SetActive(false);

					audioManager.Play(SoundNames.PLAYER_DEATH);
					audioManager.Pause(SoundNames.SHIP_ENGINE);

					shopManager.AddCash(finalScore);

					scoreboard.Show();
					scoreboard.AnimateDistanceScore(distanceScore);
					scoreboard.AnimateBonusScore(bonusScore);
					scoreboard.AnimateTopSpeed(topSpeed);
					scoreboard.AnimateFinalScore(finalScore);

					bool newScore = highscoreManager.SaveLocalHighscore(finalScore);
					bool newDistance = highscoreManager.SaveDistanceHighscore(distanceScore);

					if (newScore || newDistance)
					{
						hud.SetNewHighscoreText(Ui.NEW_HIGHSCORE);
					}

					// Now update achievements
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_1000, 1000, distanceScore);
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_10000, 10000, distanceScore);
					achievementManager.ProgressAchievement(AchievementIds.DISTANCE_FURTHER_THAN_50000, 50000, distanceScore);
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_FIVE_MILLION, 5000000, finalScore);
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_HALF_MILLION, 500000, finalScore);
					achievementManager.ProgressAchievement(AchievementIds.POINTS_OVER_MILLION, 1000000, finalScore);
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