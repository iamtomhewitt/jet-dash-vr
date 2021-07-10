namespace Utility
{
	public class Ui
	{
		public const string ALREADY_UPLOADED = "Already uploaded!";
		public const string DOWNLOADING_HIGHSCORES = "Downloading Highscores...";
		public const string ENTER_NICKNAME = "Enter a nickname!";
		public const string INVALID_NAME = "Invalid name!";
		public const string NEW_HIGHSCORE = "New Highscore!";
		public const string NO_INTERNET = "No internet connection";
		public const string POWERUP_DOUBLE_POINTS = "x2!";
		public const string POWERUP_HYPERDRIVE = "Hyperdrive!";
		public const string POWERUP_INVINCIBLE = "Invincible!";
		public const string POWERUP_JUMP = "Jump!";
		public const string POWERUP_SLOW_DOWN = "Slow Down!";
		public const string POWERUP_SPEED_UP = "Speed Up!";
		public const string SCORE_NOT_ZERO = "Score cannot be 0!";
		public const string UPLOADED = "Uploaded!";

		public static string HIGHSCORE_DOWNLOAD_ERROR(string error)
		{
			return string.Format("Could not download scores. Please try again later.\n\nError: {0}", error);
		}

		public static string HIGHSCORE_UPLOAD_ERROR(string error)
		{
			return string.Format("Could not upload score. Please try again later.\n\n{0}", error);
		}

		public static string POWERUP_BONUS_POINTS(int i)
		{
			return string.Format("+{0}!", i);
		}

		public static string PUT_ON_HEADSET(int time)
		{
			return string.Format("Put on your headset\n{0}", time);
		}

		public static string RELAUNCHING(int time)
		{
			return time < 0 ? "Relaunching" : string.Format("Relaunching in: {0}", time);
		}

		public static string SHOP_SHIP_CASH(long c)
		{
			return string.Format("Cash: {0}P", c);
		}

		public static string SHOP_SHIP_COST(long c)
		{
			return string.Format("Cost: {0}P", c);
		}

		public static string TOTAL_ACHIEVEMENT_POINTS(int achieved, int total)
		{
			return string.Format("{0}P / {1}P", achieved, total);
		}

		public static string VERSION(string v)
		{
			return string.Format("Version: {0}", v);
		}

		public static string VR_TOGGLE(bool isVR)
		{
			return string.Format("VR Mode: {0}", isVR ? "ON" : "OFF");
		}
	}
}