using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Utilities;

namespace Player
{
    public class PlayerScore : MonoBehaviour
    {
        private int bonusScore = 0;

		public static PlayerScore instance;
        private HUD hud;

		private void Awake()
		{
			instance = this;	
		}

		void Start()
        {
            hud = GetComponent<HUD>();
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

        public int GetSpeed()
        {
            return (int)GetComponent<PlayerControl>().speed;
        }

        public int GetFinalScore()
        {
            return GetBonusScore() + GetDistanceScore() + GetSpeed();
        }

		/// <summary>
		/// Saves the players score to the PlayerPrefs.
		/// </summary>
		public void SaveHighscore()
		{
			int currentHighscore = PlayerPrefs.GetInt(GameSettings.playerPrefsKey);
			int score = PlayerScore.instance.GetFinalScore();

			if (score > currentHighscore)
			{
				print("New highscore of " + score + "! Saving...");
				PlayerPrefs.SetInt(GameSettings.playerPrefsKey, score);

				// Player has got a new highscore, which obvs hasnt been uploaded yet
				PlayerPrefs.SetInt(GameSettings.uploadedKey, 0);
			}
		}
	}
}
