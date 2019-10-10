using UnityEngine;

namespace Achievement
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

		public Achievement(int id, string name, string description, int value)
		{
			this.id = id;
			this.achievementName = name;
			this.description = description;
			this.awardValue = value;
			this.unlocked = false;
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

			//Debug.Log("Achievement '" + this.achievementName + "' target: " + target + ", actual: " + actual);
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