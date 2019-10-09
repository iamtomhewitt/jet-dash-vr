using UnityEngine;

[System.Serializable]
public class Achievement
{
	public string achievementName;
	public int id;
	public int awardValue;
	public float progressPercentage;
	public bool unlocked;

	public Achievement(int id, string name, int value)
	{
		this.id = id;
		this.achievementName = name;
		this.awardValue = value;
		this.unlocked = false;
	}

	/// <summary>
	/// Progresses the achievement to a certain percentage.
	/// </summary>
	public void Progress(float percentage)
	{
		if (this.unlocked)
		{
			return;
		}

		this.progressPercentage = percentage;

		if (this.progressPercentage >= 100f)
		{
			Debug.Log("'" + this.achievementName + "' unlocked!");
			this.unlocked = true;
		}
	}

	/// <summary>
	/// Instantly unlocks an achievement.
	/// </summary>
	public void Unlock()
	{
		Progress(100f);
	}

	public int GetId()
	{
		return this.id;
	}
}