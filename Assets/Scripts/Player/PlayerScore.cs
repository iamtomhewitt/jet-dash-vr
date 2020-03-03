using Manager;
using UnityEngine;
using Utility;

namespace Player
{
	public class PlayerScore : MonoBehaviour
	{
		private int bonusScore = 0;

		public static PlayerScore instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			float acceleration = PlayerControl.instance.GetAcceleration();
			InvokeRepeating("ShowNotificationIfOnSpeedStreak", acceleration, acceleration);
		}

		private void Update()
		{
			PlayerHud.instance.SetScoreText(bonusScore);
		}

		/// <summary>
		/// Shows a notification if we are on a speed streak (every 50).
		/// </summary>
		private void ShowNotificationIfOnSpeedStreak()
		{
			float speed = PlayerControl.instance.GetSpeed();

			if (speed % 50 == 0 && !PlayerControl.instance.HasReachedMaxSpeed())
			{
				PlayerHud.instance.ShowNotification(Color.white, speed + " Speed Streak!");
				AudioManager.instance.Play(SoundNames.SPEED_STREAK);
			}
		}

		/// <summary>
		/// Saves the players distance to the PlayerPrefs.
		/// </summary>
		public void SaveDistanceHighscore()
		{
			int currentDistance = PlayerPrefs.GetInt(Constants.DISTANCE_KEY);
			int distance = GetDistanceScore();

			if (distance > currentDistance)
			{
				print("New distance of " + distance + "! Previous was " + currentDistance + ".");
				PlayerPrefs.SetInt(Constants.DISTANCE_KEY, distance);
			}
		}

		public void AddBonusPoints(int points)
		{
			bonusScore += points;
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
			return GetBonusScore() + GetDistanceScore() + PlayerControl.instance.GetSpeed();
		}
	}
}
