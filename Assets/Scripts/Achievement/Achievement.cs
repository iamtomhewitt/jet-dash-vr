﻿using UnityEngine;

namespace Achievements
{
	[System.Serializable]
	public class Achievement
	{
		public string achievementName;
		public string description;
		public int id;
		public int awardValue;
		public float progressPercentage;
		public bool unlocked;
		public bool userShown;

		public Achievement(int id, string name, string description, int value)
		{
			this.id = id;
			this.achievementName = name;
			this.description = description;
			this.awardValue = value;
			this.unlocked = false;
			this.progressPercentage = 0f;
		}

		public override string ToString()
		{
			return "Achievement: " + this.id + ", " +
					this.achievementName + ", " +
					this.description + ", " +
					this.awardValue + ", " +
					this.progressPercentage + ", " +
					this.unlocked;
		}

		/// <summary>
		/// Progresses the achievement to a certain percentage.
		/// </summary>
		public void Progress(float target, float actual)
		{
			if (this.unlocked)
			{
				Debug.Log("Achievement '" + this.achievementName + "' already unlocked!");
				return;
			}

			float percentage = (actual / target) * 100;

			// Don't want to set percentage lower than a previous value
			if (this.progressPercentage < percentage)
			{
				this.progressPercentage = percentage;
			}

			if (this.progressPercentage >= 100f)
			{
				Debug.Log("'" + this.achievementName + "' unlocked!");
				Unlock();
			}
		}

		/// <summary>
		/// Instantly unlocks an achievement.
		/// </summary>
		public void Unlock()
		{
			this.unlocked = true;
			this.progressPercentage = 100f;
		}

		public int GetId()
		{
			return this.id;
		}
	}
}