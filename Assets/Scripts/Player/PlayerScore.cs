using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Utilities;

namespace Player
{
    public class PlayerScore : MonoBehaviour
    {
        private int bonusScore = 0;

		public static PlayerScore instance;
        private PlayerHud hud;

		private void Awake()
		{
			instance = this;	
		}

		void Start()
        {
            hud = GetComponent<PlayerHud>();
		}

		void Update()
        {
            hud.SetScoreText(bonusScore);
        }

        public void AddBonusPoints(int points)
        {
            bonusScore += points;
        }

        public void DoublePoints()
        {
			if (bonusScore == 0)
				bonusScore += 500;
			else
				bonusScore *= 2;
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

		/// <summary>
		/// Saves the players score to the PlayerPrefs.
		/// </summary>
		public void SaveHighscore()
		{
			int currentHighscore = PlayerPrefs.GetInt(GameSettings.highscoreKey);
			int score = PlayerScore.instance.GetFinalScore();

			if (score > currentHighscore)
			{
				print("New highscore of " + score + "! Previous was " + currentHighscore + ".");
				PlayerPrefs.SetInt(GameSettings.highscoreKey, score);

				// Player has got a new highscore, which obvs hasnt been uploaded yet
				PlayerPrefs.SetInt(GameSettings.uploadedKey, 0);
			}
		}

		/// <summary>
		/// Saves the players distance to the PlayerPrefs.
		/// </summary>
		public void SaveDistanceHighscore()
		{
			int currentDistance = PlayerPrefs.GetInt(GameSettings.distanceKey);
			int distance = PlayerScore.instance.GetDistanceScore();

			if (distance > currentDistance)
			{
				print("New distance of " + distance + "! Previous was " + currentDistance + ".");
				PlayerPrefs.SetInt(GameSettings.distanceKey, distance);
			}
		}
	}
}
