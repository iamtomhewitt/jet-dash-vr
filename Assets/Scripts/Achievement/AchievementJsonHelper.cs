using UnityEngine;

namespace Achievement
{
	public class AchievementJsonHelper : MonoBehaviour
	{
		public static string ToJson(Achievement[] ach)
		{
			Wrapper<Achievement> wrapper = new Wrapper<Achievement>();
			wrapper.achievements = ach;
			return JsonUtility.ToJson(wrapper);
		}

		public static Achievement[] FromJson(string json)
		{
			Wrapper<Achievement> wrapper = JsonUtility.FromJson<Wrapper<Achievement>>(json);
			return wrapper.achievements;
		}

		[System.Serializable]
		private class Wrapper<T>
		{
			public T[] achievements;
		}
	}
}
