using Data;
using Manager;
using NUnit.Framework;
using Player;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests
{
	public class PlayerScoreTests
	{
		private AudioManager am;
		private AchievementManager achm;
		private GameObject player;
		private GameSettingsManager gs;
		private PlayerControl pc;
		private PlayerScore ps;
		private PlayerHud ph;
		private ShopManager sm;

		[SetUp]
		public void Setup()
		{
			am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
			achm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
			gs = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
			sm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Shop Manager")).GetComponent<ShopManager>();
			player = MonoBehaviour.Instantiate(TestConstants.GetResource("Player"));

			pc = player.GetComponent<PlayerControl>();
			ps = player.GetComponent<PlayerScore>();
			ph = player.GetComponent<PlayerHud>();
			sm.SetSelectedShipData(Resources.Load<ShipData>("Tests/Prefabs/Mock Ship"));
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(am);
			Object.Destroy(achm);
			Object.Destroy(gs);
			Object.Destroy(player);
			Object.Destroy(sm);
		}

		[UnityTest]
		public IEnumerator ShouldShowNotificationSpeedStreak()
		{
			pc.SetSpeed(50);
			ps.ShowNotificationIfOnSpeedStreak();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual("50 Speed Streak!", ph.GetPowerupNotificationText().text);
		}

		[UnityTest]
		public IEnumerator ShouldNotShowNotificationSpeedStreak()
		{
			pc.SetSpeed(98);
			ps.ShowNotificationIfOnSpeedStreak();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(string.Empty, ph.GetPowerupNotificationText().text);
		}

		[UnityTest]
		public IEnumerator ShouldNotShowNotificationSpeedStreakIfMaxSpeed()
		{
			pc.MaxSpeed();
			ps.ShowNotificationIfOnSpeedStreak();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(string.Empty, ph.GetPowerupNotificationText().text);
		}

		[UnityTest]
		public IEnumerator ShouldAddBonusPoints()
		{
			ps.AddBonusPoints(500);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(500, ps.GetBonusScore());
		}

		[UnityTest]
		public IEnumerator ShouldDoublePoints()
		{
			ps.AddBonusPoints(500);
			ps.DoublePoints();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(1000, ps.GetBonusScore());
		}

		[UnityTest]
		public IEnumerator ShouldGetDistanceScoreCorrectly()
		{
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual((int)player.transform.position.z, ps.GetDistanceScore());
		}
	}
}