using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Achievement
{
	public class AchievementManager : MonoBehaviour
	{
		[SerializeField] private Achievement[] achievements;

		public static AchievementManager instance;

		private void Awake()
		{
			if (instance)
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
				instance = this;
			}
		}

		private void Start()
		{			
			achievements = AchievementDatabase.LoadAchievementsFromFile();
		}

		/// <summary>
		/// Instantly unlocks an achievement.
		/// </summary>
		public void UnlockAchievement(int id)
		{
			GetAchievement(id).Unlock();
			AchievementDatabase.SaveAchievementsToFile(achievements);
		}

		/// <summary>
		/// Progresses an achievement.
		/// The target value is what the 100% marker is, the actual value is what the actual value was achieved.<para/>
		/// For example, say the achievement is to gain 1000 points (target), and the player achieved 750 points (actual).<para/>
		/// Then the achievement would be 75% (750/1000) complete.
		/// </summary>
		public void ProgressAchievement(int id, float target, float actual)
		{
			GetAchievement(id).Progress(target, actual);
			AchievementDatabase.SaveAchievementsToFile(achievements);
		}

		public Achievement GetAchievement(int id)
		{
			return achievements.Where(x => Equals(x.GetId(), id)).ElementAt(0);
		}

		public Achievement[] GetAchievements()
		{
			return achievements;
		}
	}
}