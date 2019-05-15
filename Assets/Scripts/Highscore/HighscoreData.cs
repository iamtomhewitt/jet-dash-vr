using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A data class to hold information retrived from the Dreamlo leaderboard.
/// </summary>
[Serializable]
public class HighscoreData
{
	public string name;
	public int score;
	public int seconds;
	public string text;
	public DateTime date;
}
