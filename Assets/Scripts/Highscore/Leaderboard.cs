using System;

namespace Highscore
{
	/// <summary>
	/// A class holding all of the highscores from the Dreamlo website. It is used to convert the JSON from Dreamlo into objects.
	/// It is a representation of the online leaderboard viewed from a browser.
	/// </summary>
	[Serializable]
	public class Leaderboard
	{
		public HighscoreData[] entry;
	}

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
}