using Manager;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests
{
	public class PlayerControlTests
	{
		private float WAIT_TIME = 0.1f;

		private GameObject player;
		private GameSettingsManager gs;
		private ShopManager sm;
		private AudioManager am;

		[SetUp]
		public void Setup()
		{
			gs = MonoBehaviour.Instantiate(GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
			sm = MonoBehaviour.Instantiate(GetResource("Managers/Shop Manager")).GetComponent<ShopManager>();
			am = MonoBehaviour.Instantiate(GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
			player = MonoBehaviour.Instantiate(GetResource("Player"));
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(player);
		}

		[UnityTest]
		public IEnumerator PlayerMovesForward()
		{
			float z = player.transform.position.z;
			yield return new WaitForSeconds(WAIT_TIME);
			Assert.AreNotEqual(player.transform.position.z, z);
		}

		private GameObject GetResource(string path)
		{
			return Resources.Load<GameObject>("Tests/Prefabs/" + path);
		}
	}
}
