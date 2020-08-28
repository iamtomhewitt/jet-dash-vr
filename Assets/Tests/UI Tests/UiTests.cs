using Manager;
using NUnit.Framework;
using System.Collections;
using UI;
using UnityEngine.TestTools;
using UnityEngine;
using Utility;

namespace Tests
{
	public class UiTests
	{
		private AchievementManager am;
		private GameSettingsManager gs;
		private HighscoreManager hm;
		private NotificationIcon notificationIcon;

		[SetUp]
		public void Setup()
		{
			am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
			gs = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
			hm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Highscore Manager")).GetComponent<HighscoreManager>();
			notificationIcon = MonoBehaviour.Instantiate(TestConstants.GetResource("Notification Icon").GetComponent<NotificationIcon>());
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(am.gameObject);
			Object.Destroy(gs.gameObject);
			Object.Destroy(hm.gameObject);
			Object.Destroy(notificationIcon.gameObject);
		}

		[UnityTest]
		public IEnumerator ShouldShowIcon()
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.HIGHSCORE, 50);
			hm.SaveLocalHighscore(100);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			notificationIcon.Start();
			Assert.AreEqual(true, notificationIcon.isActiveAndEnabled);
		}

		[UnityTest]
		public IEnumerator ShouldHideIconAfterUploading()
		{
			// Simulate upload
			PlayerPrefs.SetInt(PlayerPrefKeys.NEW_DISTANCE, Constants.NO);
			PlayerPrefs.SetInt(PlayerPrefKeys.NEW_HIGHSCORE, Constants.NO);

			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			notificationIcon.Start();
			Assert.AreEqual(false, notificationIcon.isActiveAndEnabled);
		}

		[UnityTest]
		public IEnumerator ShouldTurnOn()
		{
			notificationIcon.TurnOn();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(true, notificationIcon.isActiveAndEnabled);
		}

		[UnityTest]
		public IEnumerator ShouldTurnOff()
		{
			notificationIcon.TurnOff();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(false, notificationIcon.isActiveAndEnabled);
		}
	}
}