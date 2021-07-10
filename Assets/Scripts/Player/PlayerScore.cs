using Manager;
using UnityEngine;
using Utility;

namespace Player
{
	public class PlayerScore : MonoBehaviour
	{
		private PlayerControl playerControl;
		private PlayerHud hud;

		private int bonusScore = 0;
		private int speedStreak = 50;

		private bool shown50Streak = false;
		private bool shown100Streak = false;
		private bool shownMaxSpeedStreak = false;

		private void Start()
		{
			playerControl = GetComponent<PlayerControl>();
			hud = GetComponent<PlayerHud>();

			float acceleration = playerControl.GetAcceleration();
			InvokeRepeating("ShowNotificationIfOnSpeedStreak", acceleration, acceleration);
		}

		/// <summary>
		/// Shows a notification if we are on a speed streak.
		/// </summary>
		public void ShowNotificationIfOnSpeedStreak()
		{
			float speed = playerControl.GetSpeed();

			if (speed > 50f && !shown50Streak)
			{
				hud.ShowNotification(Color.white, "50 Speed Streak!");
				AudioManager.instance.Play(SoundNames.SPEED_STREAK);
				shown50Streak = true;
			}

			if (speed > 100f && !shown100Streak)
			{
				hud.ShowNotification(Color.white, "100 Speed Streak!");
				AudioManager.instance.Play(SoundNames.SPEED_STREAK);
				shown100Streak = true;
			}

			if (playerControl.HasReachedMaxSpeed() && !shownMaxSpeedStreak)
			{
				hud.ShowNotification(Color.white, "Max Speed!");
				AudioManager.instance.Play(SoundNames.SPEED_STREAK);
				shownMaxSpeedStreak = true;
			}
		}

		public void AddBonusPoints(int points)
		{
			bonusScore += points;
			hud.SetScoreText(bonusScore);
		}

		public void DoublePoints()
		{
			if (bonusScore == 0)
			{
				bonusScore += 500;
			}
			else
			{
				bonusScore *= 2;
			}
		}

		public int GetBonusScore()
		{
			return bonusScore;
		}

		public int GetDistanceScore()
		{
			return (int)transform.position.z;
		}

		public int GetFinalScore()
		{
			bool shouldDoubleScore = ShopManager.instance.GetSelectedShipData().GetShipName().Equals(Tags.CELLEX);
			int finalScore = GetBonusScore() + GetDistanceScore() + playerControl.GetSpeed();
			return shouldDoubleScore ? finalScore * 2 : finalScore;
		}
	}
}