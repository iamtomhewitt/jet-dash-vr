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

		private void Start()
		{
			playerControl = GetComponent<PlayerControl>();
			hud = GetComponent<PlayerHud>();

			float acceleration = playerControl.GetAcceleration();
			InvokeRepeating("ShowNotificationIfOnSpeedStreak", acceleration, acceleration);
		}

		/// <summary>
		/// Shows a notification if we are on a speed streak (every 50).
		/// </summary>
		public void ShowNotificationIfOnSpeedStreak()
		{
			float speed = playerControl.GetSpeed();

			if (speed % speedStreak == 0 && !playerControl.HasReachedMaxSpeed())
			{			
				hud.ShowNotification(Color.white, speed + " Speed Streak!");
				AudioManager.instance.Play(SoundNames.SPEED_STREAK);
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