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
		List<Achievement> list = new List<Achievement>();
		list.Add(new Achievement(1, "Die", 5));
		list.Add(new Achievement(2, "Get A New Highscore", 30));
		list.Add(new Achievement(3, "Upload A Highscore", 15));
		list.Add(new Achievement(4, "Play In VR Mode", 5));
		list.Add(new Achievement(5, "Get A Distance Further Than 1000", 5));
		list.Add(new Achievement(6, "Get A Distance Further Than 10000", 30));
		list.Add(new Achievement(7, "Get A Distance Further Than 100000", 100));
		list.Add(new Achievement(8, "Fly Through Bonus Points", 5));
		list.Add(new Achievement(9, "Fly Through Double Points", 5));
		list.Add(new Achievement(10, "Become Invincible", 5));
		list.Add(new Achievement(11, "Fly Through An Obstacle Whilst Invincible", 15));
		list.Add(new Achievement(12, "Achieve Max Speed", 25));
		return list.ToArray();
	}
}
