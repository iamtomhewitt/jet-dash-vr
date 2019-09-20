﻿using UnityEngine;
using Utility;

namespace Player
{
	public class PlayerScore : MonoBehaviour
    {
		private PlayerHud hud;

		private int bonusScore = 0;

		public static PlayerScore instance;

		private void Awake()
		{
			instance = this;	
		}

		private void Start()
        {
            hud = GetComponent<PlayerHud>();
		}

		private void Update()
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
	}
}
