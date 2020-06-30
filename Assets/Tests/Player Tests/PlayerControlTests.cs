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
		private float WAIT_TIME = 0.1f;

		private AudioManager am;
		private AchievementManager achm;
		private GameObject player;
		private GameSettingsManager gs;
		private PlayerControl pc;
		private ShopManager sm;

		[SetUp]
		public void Setup()
		{
			am = MonoBehaviour.Instantiate(GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
			achm = MonoBehaviour.Instantiate(GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
			gs = MonoBehaviour.Instantiate(GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
			sm = MonoBehaviour.Instantiate(GetResource("Managers/Shop Manager")).GetComponent<ShopManager>();
			player = MonoBehaviour.Instantiate(GetResource("Player"));
			pc = player.GetComponent<PlayerControl>();
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
			yield return new WaitForSeconds(WAIT_TIME);
			Assert.AreNotEqual(player.transform.position.z, z);
		}

		[UnityTest]
		public IEnumerator ShouldIncreaseSpeed()
		{
			int speed = pc.GetSpeed();
			pc.IncreaseSpeed();
			yield return new WaitForSeconds(WAIT_TIME);
			Assert.AreNotEqual(pc.GetSpeed(), speed);
		}

		[UnityTest]
		public IEnumerator ShouldNotIncreaseSpeedWhenMaxSpeedReached()
		{
			pc.MaxSpeed();
			int speed = pc.GetSpeed();
			pc.IncreaseSpeed();
			yield return new WaitForSeconds(WAIT_TIME);
			Assert.AreEqual(pc.GetSpeed(), speed);
			Assert.True(pc.HasReachedMaxSpeed());
		}

		[UnityTest]
		public IEnumerator ShouldStopMoving()
		{
			pc.StopMoving();
			yield return new WaitForSeconds(WAIT_TIME);
			Assert.AreEqual(pc.GetSpeed(), 0f);
			Assert.AreEqual(pc.GetTurningSpeed(), 0f);
		}

		[UnityTest]
		public IEnumerator ShouldResetYPosition()
		{
			player.transform.SetYPosition(-100f);
			pc.CheckYPosition();
			Assert.GreaterOrEqual(player.transform.position.y, 0f);
			yield return new WaitForSeconds(WAIT_TIME);
		}

		private GameObject GetResource(string path)
		{
			return Resources.Load<GameObject>("Tests/Prefabs/" + path);
		}
	}
}