using Manager;
using NUnit.Framework;
using Player;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using SpawnableObject.Powerups;

namespace Tests
{
	public class PowerupTests
	{
		private AchievementManager achm;
		private AudioManager am;
		private GameObject bp;
		private GameObject dpp;
		private GameObject hp;
		private GameObject ip;
		private GameObject jp;
		private GameObject player;
		private GameObject sdp;
		private GameObject sup;
		private GameSettingsManager gs;
		private PlayerCollision pc;
		private PlayerControl pcn;
		private PlayerScore ps;
		private ShopManager sm;

		[SetUp]
		public void Setup()
		{
			am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
			achm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
			gs = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
			player = MonoBehaviour.Instantiate(TestConstants.GetResource("Player"));
			player.transform.position = new Vector3(0f, 0f - 1000f);

			bp = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Bonus Points"));
			dpp = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Double Points"));
			hp = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Hyperdrive"));
			ip = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Invincibility"));
			jp = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Jump"));
			sdp = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Slow Down"));
			sm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Shop Manager")).GetComponent<ShopManager>();
			sup = MonoBehaviour.Instantiate(TestConstants.GetResource("Powerups/Speed Up"));

			pc = player.GetComponent<PlayerCollision>();
			pcn = player.GetComponent<PlayerControl>();
			ps = player.GetComponent<PlayerScore>();
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(achm);
			Object.Destroy(am);
			Object.Destroy(bp);
			Object.Destroy(dpp);
			Object.Destroy(gs);
			Object.Destroy(hp);
			Object.Destroy(ip);
			Object.Destroy(jp);
			Object.Destroy(player);
			Object.Destroy(sdp);
			Object.Destroy(sm);
			Object.Destroy(sup);
		}

		[UnityTest]
		public IEnumerator ShouldAddBonusPoints()
		{
			MakePlayerCollideWith(bp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(500, ps.GetBonusScore());
		}

		[UnityTest]
		public IEnumerator ShouldGivePointsWhenCollidingWithDoublePointsAndZeroScore()
		{
			MakePlayerCollideWith(dpp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(500, ps.GetBonusScore());
		}

		[UnityTest]
		public IEnumerator ShouldDoublePoints()
		{
			ps.AddBonusPoints(1000);
			MakePlayerCollideWith(dpp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(2000, ps.GetBonusScore());
		}

		[UnityTest]
		public IEnumerator ShouldMakePlayerInvincible()
		{
			MakePlayerCollideWith(ip);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.True(pc.IsGodMode());
			Assert.True(pc.GetShield().activeSelf);
		}

		[UnityTest]
		public IEnumerator ShouldIncreaseSpeed()
		{
			pcn.SetSpeed(50f);
			float s = pcn.GetSpeed();
			MakePlayerCollideWith(sup);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(70f, pcn.GetSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldNotIncreaseSpeedIfReachedMaxSpeed()
		{
			pcn.MaxSpeed();
			MakePlayerCollideWith(sup);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(200f, pcn.GetSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldNotDecreaseSpeedIfZeroSpeed()
		{
			pcn.SetSpeed(0f);
			MakePlayerCollideWith(sdp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(0f, pcn.GetSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldDecreaseSpeed()
		{
			pcn.SetSpeed(50f);
			float s = pcn.GetSpeed();
			MakePlayerCollideWith(sdp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(40f, pcn.GetSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldMakePlayerGodModeWhenHyperdriving()
		{
			MakePlayerCollideWith(hp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.True(pc.IsGodMode());
		}

		[UnityTest]
		public IEnumerator ShouldMakePlayerJump()
		{
			float y = player.transform.position.y;
			MakePlayerCollideWith(jp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.GreaterOrEqual(player.transform.position.y, y);
		}

		[UnityTest]
		public IEnumerator ShouldRelocateAfterCollision()
		{
			Vector3 pos = bp.transform.position;
			MakePlayerCollideWith(bp);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreNotEqual(pos, bp.transform.position);
		}

		[UnityTest]
		public IEnumerator ShouldRelocateIfBehindPlayer()
		{
			bp.transform.position = new Vector3(0f, 0f, -2000f);
			Vector3 pos = bp.transform.position;
			bp.GetComponent<BonusPoints>().RelocateIfBehindPlayer();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreNotEqual(pos, bp.transform.position);
			Assert.GreaterOrEqual(bp.transform.position.z, player.transform.position.z);
		}

		private void MakePlayerCollideWith(GameObject gameObject)
		{
			player.transform.position = gameObject.transform.position;
		}
	}
}