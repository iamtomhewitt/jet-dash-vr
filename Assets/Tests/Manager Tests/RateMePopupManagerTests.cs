using Manager;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using static Manager.RateMePopupManager;

namespace Tests
{
	public class RateMePopupManagerTests
	{
		private RateMePopupManager rm;

		[SetUp]
		public void Setup()
		{
			rm = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Tests/Managers/Rate Me Popup Manager")).GetComponent<RateMePopupManager>();
		}

		[TearDown]
		public void Teardown()
		{
			Object.Destroy(rm.gameObject);
		}

		[UnityTest]
		public IEnumerator ShouldBeAbleToShowRateMePopup()
		{
			IncreaseCounter(30);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.True(rm.CanShowRateMePopup());
		}

		[UnityTest]
		public IEnumerator ShouldNotBeAbleToShowRateMePopup()
		{
			IncreaseCounter(10);
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.False(rm.CanShowRateMePopup());
		}

		[UnityTest]
		public IEnumerator ShouldResetCounterWhenShowingPopup()
		{
			IncreaseCounter(30);
			rm.ShowRateMePopup();
			yield return new WaitForSeconds(TestConstants.WAIT_TIME);
			Assert.AreEqual(0, rm.GetShowCount());
		}

		private void IncreaseCounter(int times)
		{
			for (int i = 0; i < times; i++)
			{
				rm.IncreaseShowCount();
			}
		}

		// Note - cannot test screen orientation stuff as not testable on non-mobile device.
	}
}