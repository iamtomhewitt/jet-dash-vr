using Manager;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace Achievements
{
	/// <summary>
	/// Used to show the achievements on the achivements scene.
	/// </summary>
	public class AchievementDisplayHelper : MonoBehaviour
	{
		public AchievementEntry entryPrefab;
		public Transform entryParent;
		public Text totalPointsText;

		private void Start()
		{
			DisplayAchievements(AchievementManager.instance.GetAchievements());
			CalculateTotalAchievementPoints();
		}

		private void DisplayAchievements(Achievement[] achievements)
		{
			foreach (Achievement a in achievements)
			{
				AchievementEntry entry = Instantiate(entryPrefab, entryParent).GetComponent<AchievementEntry>();
				entry.Populate(a.achievementName, a.description, a.progressPercentage, a.awardValue);
			}
		}

		private void CalculateTotalAchievementPoints()
		{
			Achievement[] achievements = AchievementManager.instance.GetAchievements();
			int total = 0;
			int achieved = 0;

			foreach (Achievement a in achievements)
			{
				if (a.unlocked)
				{
					achieved += a.awardValue;
				}

				total += a.awardValue;
			}

			totalPointsText.SetText(Ui.TOTAL_ACHIEVEMENT_POINTS(achieved, total));
		}
	}
}