using System;

/// <summary>
/// A class holding all of the highscores from the Dreamlo website. It is used to convert the JSON from Dreamlo into objects.
/// It is a representation of the online leaderboard viewed from a browser.
/// </summary>
[Serializable]
public class Leaderboard
{
	public HighscoreData[] entry;
}