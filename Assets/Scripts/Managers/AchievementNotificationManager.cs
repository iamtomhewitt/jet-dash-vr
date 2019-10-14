using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Achievements;

namespace Manager
{
	/// <summary>
	/// Displays Toast like notifications for unlocking achievements.
	/// </summary>
	public class AchievementNotificationManager : MonoBehaviour
	{
		[SerializeField] private GameObject notification;
		[SerializeField] private Text achievementName;
		[SerializeField] private Text achievementDescription;
		[SerializeField] private Text achievementValue;
		[SerializeField] private Animator animator;

		private Queue notificationQueue = new Queue();

		private bool showingAchievement = false;

		private const string ACHIEVEMENT_HIDE = "Achievement Hide";
		private const string ACHIEVEMENT_SHOW = "Achievement Show";
		private const float SHOW_TIME = 3f;
		private const float TIME_BETWEEN_NOTIFICATIONS = 2f;

		public static AchievementNotificationManager instance;

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

		/// <summary>
		/// Queues an achievement to be shown.
		/// </summary>
		public void AddToNotificationQueue(Achievement achievement)
		{
			notificationQueue.Enqueue(achievement);
		}

		/// <summary>
		/// Shows the next achievement notification in the queue.
		/// </summary>
		public void ShowNotification()
		{
			StartCoroutine(ShowNotificationRoutine());
		}

		private IEnumerator ShowNotificationRoutine()
		{
			while (showingAchievement)
			{
				yield return null;
			}

			animator.Play(ACHIEVEMENT_SHOW);
			AudioManager.instance.Play(SoundNames.ACHIEVEMENT_UNLOCKED);
			showingAchievement = true;

			// Get the next achievement in the queue
			Achievement a = (Achievement)notificationQueue.Dequeue();
			achievementName.text = a.achievementName;
			achievementDescription.text = a.description;
			achievementValue.text = a.awardValue.ToString() + "P";

			yield return new WaitForSeconds(SHOW_TIME);

			animator.Play(ACHIEVEMENT_HIDE);

			yield return new WaitForSeconds(TIME_BETWEEN_NOTIFICATIONS);

			showingAchievement = false;
		}
	}
}
