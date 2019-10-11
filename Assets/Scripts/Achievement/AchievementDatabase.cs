using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Achievement
{
	/// <summary>
	/// Responsible for file based operations, saving and loading achievements.
	/// </summary>
	public class AchievementDatabase : MonoBehaviour
	{
		private static string achievementFilePath = Path.Combine(Application.persistentDataPath, "achievements.json");

		/// <summary>
		/// Returns the achivements saved in a JSON file.
		/// </summary>
		public static Achievement[] LoadAchievementsFromFile()
		{
			Achievement[] achievements;

			// Generate a new set of achievements if the file does not exist
			if (!File.Exists(achievementFilePath))
			{
				achievements = GenerateSetOfAchievements();
				SaveAchievementsToFile(achievements);
			}

			string fileContent = File.ReadAllText(achievementFilePath);

			return AchievementJsonHelper.FromJson(fileContent);
		}

		/// <summary>
		/// Saves the achivements as JSON to file.
		/// </summary>
		public static void SaveAchievementsToFile(Achievement[] achievements)
		{
			string json = AchievementJsonHelper.ToJson(achievements);

			StreamWriter writer = new StreamWriter(achievementFilePath, false);
			writer.WriteLine(json);
			writer.Close();
		}

		/// <summary>
		/// Used when the achivement file cannot be found / does not exist.
		/// Generates an array of achievements.
		/// </summary>
		private static Achievement[] GenerateSetOfAchievements()
		{
			return new List<Achievement>
			{
				new Achievement(AchievementIds.DIE, "Die", "Crash into an obstacle.", 5),
				new Achievement(AchievementIds.NEW_HIGHSCORE, "Newbie", "Get A New Highscore", 30),
				new Achievement(AchievementIds.UPLOAD_HIGHSCORE, "Welcome To The Club", "Upload A Highscore", 15),
				new Achievement(AchievementIds.PLAY_IN_VR, "Seeing Double", "Play In VR Mode", 5),
				new Achievement(AchievementIds.DISTANCE_FURTHER_THAN_1000, "Going Places", "Get a distance further than 1000.", 5),
				new Achievement(AchievementIds.DISTANCE_FURTHER_THAN_10000, "To The Moon!", "Get a distance further than 10,000.", 30),
				new Achievement(AchievementIds.DISTANCE_FURTHER_THAN_50000, "Sky Is The Limit", "Get a distance further than 50,000.", 200),
				new Achievement(AchievementIds.FLY_THROUGH_BONUS_POINTS, "BONUS", "Fly Through Bonus Points", 5),
				new Achievement(AchievementIds.FLY_THROUGH_DOUBLE_POINTS, "That's A Double", "Fly Through Double Points", 5),
				new Achievement(AchievementIds.BECOME_INVINCIBLE, "The Power!", "Become Invincible", 5),
				new Achievement(AchievementIds.FLY_THROUGH_OBSTACLE_WHEN_INVINCIBLE, "Indestructable", "Fly Through An Obstacle Whilst Invincible", 15),
				new Achievement(AchievementIds.GET_MAX_SPEED, "Turbo", "Achieve Max Speed", 25),
				new Achievement(AchievementIds.POINTS_OVER_HALF_MILLION, "What?", "Earn 500,000 points", 100),
				new Achievement(AchievementIds.POINTS_OVER_MILLION, "IMPOSSIBLE!", "Earn 1,000,000 points", 500),
				new Achievement(AchievementIds.POINTS_OVER_FIVE_MILLION, "UNFRIGGENBELIEVEABLE", "Earn 5,000,000 points", 1000)
			}.ToArray();
		}
	}
}
