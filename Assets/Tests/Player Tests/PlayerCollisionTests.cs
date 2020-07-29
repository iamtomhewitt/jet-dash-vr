using Data;
using Manager;
using NUnit.Framework;
using Player;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests
{
	public class PlayerCollisionTests
	{
		private AchievementManager achm;
		private AudioManager am;
		private GameObject obstacle;
		private GameObject player;
		private GameObject powerup;
		private GameSettingsManager gs;
		private HighscoreManager hm;
		private PlayerCollision pc;
		private ShopManager sm;

		[SetUp]
		public void Setup()
		{
			achm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
			am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
			gs = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
			hm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Highscore Manager").GetComponent<HighscoreManager>());
			obstacle = MonoBehaviour.Instantiate(TestConstants.GetResource("Obstacle"));
			player = MonoBehaviour.Instantiate(TestConstants.GetResource("Player"));
			powerup = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerup"));
			sm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Shop Manager")).GetComponent<ShopManager>();

			pc = player.GetComponent<PlayerCollision>();
			sm.SetSelectedShipData(Resources.Load<ShipData>("Tests/Mock Ship"));
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(achm);
			Object.Destroy(am);
			Object.Destroy(gs);
			Object.Destroy(hm);
			Object.Destroy(obstacle);
			Object.Destroy(player);
			Object.Destroy(powerup);
			Object.Destroy(sm);
		}

		[UnityTest]
		public IEnumerator ShouldNotDieIfInvincible()
		{
			pc.SetInvincible(true);
			MakePlayerCollideWith(obstacle);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.IsFalse(pc.IsDead());
		}

		[UnityTest]
		public IEnumerator ShouldNotDieIfInHyperdrive()
		{
			pc.SetHyperdriveEnabled(true);
			MakePlayerCollideWith(obstacle);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.IsFalse(pc.IsDead());
		}

		[UnityTest]
		public IEnumerator ShouldDieWhenCollidingWithObstacle()
		{
			MakePlayerCollideWith(obstacle);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.IsTrue(pc.IsDead());
		}

		[UnityTest]
		public IEnumerator ShouldRelocatePowerupWhenColliding()
		{
			Vector3 pos = powerup.transform.position;
			MakePlayerCollideWith(powerup);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreNotEqual(powerup.transform.position, pos);
		}

		[UnityTest]
		public IEnumerator ShouldReturnShield()
		{
			GameObject shield = pc.GetShield();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.NotNull(shield);
		}

		private void MakePlayerCollideWith(GameObject gameObject)
		{
			player.transform.position = gameObject.transform.position;
		}
	}
}