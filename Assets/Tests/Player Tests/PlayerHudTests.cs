using Data;
using Manager;
using NUnit.Framework;
using Player;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests
{
	public class PlayerHudTests
	{
		private AudioManager am;
		private AchievementManager achm;
		private GameObject player;
		private GameSettingsManager gs;
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
		public IEnumerator ShouldSetTextsCorrectly()
		{
			ph.SetDistanceText(1f);
			ph.SetScoreText(100);
			ph.SetSpeedText(100f);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreNotEqual("01", ph.GetDistanceText());
			Assert.AreNotEqual("100", ph.GetScoreText());
			Assert.AreNotEqual("100", ph.GetSpeedText());
		}
	}
}