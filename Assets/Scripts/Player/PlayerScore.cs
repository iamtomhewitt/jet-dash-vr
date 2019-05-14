﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Player
{
    public class PlayerScore : MonoBehaviour
    {
        private int bonusScore = 0;
        private HUD hud;

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
    }
}
