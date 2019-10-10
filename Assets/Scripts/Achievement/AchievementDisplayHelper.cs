using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Achievement
{
	public class AchievementDisplayHelper : MonoBehaviour
	{
		public AchievementEntry entryPrefab;
		public Transform entryParent;
		public Text totalPointsText;

		private void Start()
		{
			StartCoroutine(DisplayAchievements(AchievementManager.instance.GetAchievements()));
			CalculateTotalAchievementPoints();
		}

		public IEnumerator DisplayAchievements(Achievement[] achievements)
		{
			yield return new WaitForSeconds(0.5f);

			for (int i = 0; i < achievements.Length; i++)
			{
				Achievement a = achievements[i];
				AchievementEntry entry = Instantiate(entryPrefab, entryParent).GetComponent<AchievementEntry>();
				entry.Populate(a.achievementName, a.description, a.progressPercentage, a.awardValue);
			}
		}

		public void CalculateTotalAchievementPoints()
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

			totalPointsText.text = achieved.ToString() + "P / " + total.ToString() + "P";
		}
	}
}
