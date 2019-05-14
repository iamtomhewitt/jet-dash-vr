using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreEntry : MonoBehaviour
{
	/// <summary>
	/// Used in the inspector to keep track of 2 text fields for displaying highscore information.
	/// </summary>	
	[SerializeField] private Text rank;
	[SerializeField] private Text username;
	[SerializeField] private Text score;

	public void SetName(string n)
	{
		username.text = n;
	}

	public void SetScore(int s)
	{
		score.text = s.ToString();
	}

	public int GetScore()
	{
		return int.Parse(score.text);
	}

	public void SetRank(string r)
	{
		rank.text = r;
	}
}
