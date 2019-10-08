using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
	public int id;
	public string achievementName;
	public int awardValue;
	public float progressPercentage;

	public Achievement(int id, string name, int value)
	{
		this.id = id;
		this.achievementName = name;
		this.awardValue = value;
	}
}

/// <summary>
/// Achievement ideas:
/// 
/// - Die
/// - Get a new highscore
/// - Upload a highscore
/// - Play in VR mode
/// - Get a distance further than X, then Y, then Z
/// - Get bonus points
/// - Get double points
/// - Get invincibility
/// - Fly through an obstacle whilst invincible
/// - Achieve max speed
/// 
/// </summary>

