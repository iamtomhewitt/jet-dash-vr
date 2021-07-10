namespace Highscores
{
	public class Highscore
	{
		private string name;
		private string ship;
		private string date;
		private int score;
		private int distance;
		private bool vrMode;

		public Highscore(string name, int score, int distance, string ship, bool vrMode, string date)
		{
			this.name = name;
			this.score = score;
			this.distance = distance;
			this.ship = ship;
			this.vrMode = vrMode;
			this.date = date;
		}

		public override string ToString()
		{
			return "{name=" + name + ", " +
				"score=" + score + ", " +
				"distance=" + distance + ", " +
				"ship=" + ship + ", " +
				"vrMode=" + vrMode + ", " +
				"date=" + date + "}";
		}

		public string GetName()
		{
			return name;
		}

		public int GetScore()
		{
			return score;
		}

		public int GetDistance()
		{
			return distance;
		}

		public string GetShip()
		{
			return ship;
		}

		public bool isVrMode()
		{
			return vrMode;
		}

		public string GetDate()
		{
			return date;
		}
	}
}