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

		public static PlayerScore instance;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			playerControl = PlayerControl.instance;
			hud = PlayerHud.instance;

			float acceleration = playerControl.GetAcceleration();
			InvokeRepeating("ShowNotificationIfOnSpeedStreak", acceleration, acceleration);
		}

		private void Update()
		{
			hud.SetScoreText(bonusScore);
		}

		/// <summary>
		/// Shows a notification if we are on a speed streak (every 50).
		/// </summary>
		private void ShowNotificationIfOnSpeedStreak()
		{
			float speed = playerControl.GetSpeed();

			if (speed % speedStreak == 0 && !playerControl.HasReachedMaxSpeed())
			{
				hud.ShowNotification(Color.white, speed + " Speed Streak!");
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
				Debug.Log("New distance of " + distance + "! Previous was " + currentDistance + ".");
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
			return GetBonusScore() + GetDistanceScore() + playerControl.GetSpeed();
		}
	}
}