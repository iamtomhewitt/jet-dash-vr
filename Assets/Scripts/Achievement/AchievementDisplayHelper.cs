using System.Collections;
using UnityEngine;

namespace Achievement
{
	public class AchievementDisplayHelper : MonoBehaviour
	{
		public AchievementEntry entryPrefab;
		public Transform entryParent;

		private void Start()
		{
			StartCoroutine(DisplayAchievements(AchievementManager.instance.GetAchievements()));
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
	}
}
