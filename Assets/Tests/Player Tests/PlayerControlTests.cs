using Data;
using Manager;
using NUnit.Framework;
using Player;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using Utility;

namespace Tests
{
	public class PlayerControlTests
	{
		private AudioManager am;
		private AchievementManager achm;
		private GameObject player;
		private GameSettingsManager gs;
		private PlayerControl pc;
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
			sm.SetSelectedShipData(Resources.Load<ShipData>("Tests/Mock Ship"));
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
		public IEnumerator ShouldMoveForward()
		{
			float z = player.transform.position.z;
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreNotEqual(z, player.transform.position.z);
		}

		[UnityTest]
		public IEnumerator ShouldIncreaseSpeed()
		{
			int speed = pc.GetSpeed();
			pc.IncreaseSpeed();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreNotEqual(speed, pc.GetSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldNotIncreaseSpeedWhenMaxSpeedReached()
		{
			pc.MaxSpeed();
			int speed = pc.GetSpeed();
			pc.IncreaseSpeed();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(speed, pc.GetSpeed());
			Assert.True(pc.HasReachedMaxSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldStopMoving()
		{
			pc.StopMoving();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(0f, pc.GetSpeed());
			Assert.AreEqual(0f, pc.GetTurningSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldResetYPosition()
		{
			player.transform.SetYPosition(-100f);
			pc.CheckYPosition();
			Assert.LessOrEqual(0f, player.transform.position.y);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
		}

		[UnityTest]
		public IEnumerator ShouldReadGameManagerSettingsCorrectly()
		{
			gs.SetVrMode(true);
			pc.Start();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(GameObject.FindGameObjectWithTag("MainCamera"), null); // If VR mode, then main camera turned off
		}

		[UnityTest]
		public IEnumerator ShouldReadShopManagerSettingsCorrectly()
		{
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(20, pc.GetSpeed());
			Assert.AreEqual(2f, pc.GetAcceleration());
			Assert.AreEqual(20f, pc.GetTurningSpeed());
			Assert.NotNull(GameObject.FindGameObjectWithTag(sm.GetSelectedShipData().GetShipName()));
		}
	}
}