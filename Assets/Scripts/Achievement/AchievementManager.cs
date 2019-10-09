using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class AchievementManager : MonoBehaviour
{
	public Achievement[] achievements;

	private string achievementFilePath;

	private void Start()
	{
		achievementFilePath = Path.Combine(Application.persistentDataPath, "achievements.json");
		achievements = LoadAchievementsFromFile();
	}

	/// <summary>
	/// Returns the achivements saved in a JSON file.
	/// </summary>
	public Achievement[] LoadAchievementsFromFile()
	{
		// Generate a new set of achievements if the file does not exist
		if (!File.Exists(achievementFilePath))
		{
			achievements = GenerateSetOfAchievements();
			SaveAchievementsToFile();
		}

		string fileContent = File.ReadAllText(achievementFilePath);

		return AchievementJsonHelper.FromJson(fileContent);
	}

	/// <summary>
	/// Saves the achivements as JSON to file.
	/// </summary>
	public void SaveAchievementsToFile()
	{
		string json = AchievementJsonHelper.ToJson(achievements);

		StreamWriter writer = new StreamWriter(achievementFilePath, false);
		writer.WriteLine(json);
		writer.Close();
	}

	/// <summary>
	/// Instantly unlocks an achievement.
	/// </summary>
	public void UnlockAchievement(int id)
	{
		GetAchievement(id).Unlock();
		SaveAchievementsToFile();
	}

	/// <summary>
	/// Progresses an achievement.
	/// </summary>
	public void ProgressAchievement(int id, float percentage)
	{
		GetAchievement(id).Progress(percentage);
		SaveAchievementsToFile();
	}

	public Achievement GetAchievement(int id)
	{
		return achievements.Where(x => Equals(x.GetId(), id)).ElementAt(0);
	}

	/// <summary>
	/// Used when the achivement file cannot be found / does not exist.
	/// Generates an array of achievements.
	/// </summary>
	private Achievement[] GenerateSetOfAchievements()
	{
		return new List<Achievement>
		{
			new Achievement(AchievementIds.DIE, "Die", 5),
			new Achievement(AchievementIds.NEW_HIGHSCORE, "Get A New Highscore", 30),
			new Achievement(AchievementIds.UPLOAD_HIGHSCORE, "Upload A Highscore", 15),
			new Achievement(AchievementIds.PLAY_IN_VR, "Play In VR Mode", 5),
			new Achievement(AchievementIds.DISTANCE_FURTHER_THAN_1000, "Get A Distance Further Than 1000", 5),
			new Achievement(AchievementIds.DISTANCE_FURTHER_THAN_10000, "Get A Distance Further Than 10000", 30),
			new Achievement(AchievementIds.DISTANCE_FURTHER_THAN_100000, "Get A Distance Further Than 100000", 100),
			new Achievement(AchievementIds.FLY_THROUGH_BONUS_POINTS, "Fly Through Bonus Points", 5),
			new Achievement(AchievementIds.FLY_THROUGH_DOUBLE_POINTS, "Fly Through Double Points", 5),
			new Achievement(AchievementIds.BECOME_INVINCIBLE, "Become Invincible", 5),
			new Achievement(AchievementIds.FLY_THROUGH_OBSTACLE_WHEN_INVINCIBLE, "Fly Through An Obstacle Whilst Invincible", 15),
			new Achievement(AchievementIds.GET_MAX_SPEED, "Achieve Max Speed", 25)
		}.ToArray();
	}
}
