using Manager;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using Utility;

namespace Tests
{
    public class ShopManagerTests
    {
        private AudioManager am;
        private ShopManager sm;

        [SetUp]
        public void Setup()
        {
            PlayerPrefs.DeleteAll();
            am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
            sm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Shop Manager")).GetComponent<ShopManager>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(am.gameObject);
            Object.Destroy(sm.gameObject);
        }

        [UnityTest]
        public IEnumerator ShouldPurchaseShip()
        {
            sm.AddCash(100000000);
            sm.PurchaseShip("Cellex");
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreEqual("Cellex", sm.GetSelectedShipData().GetShipName());
            Assert.True(sm.IsShipUnlocked(sm.GetSelectedShipData()));
        }

        [UnityTest]
        public IEnumerator ShouldNotPurchaseShipIfAlreadyUnlocked()
        {
            PlayerPrefs.SetInt("Cellex" + PlayerPrefKeys.UNLOCKED, Constants.YES);
            sm.AddCash(100000000);
            sm.PurchaseShip("Cellex");
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreEqual("Rescate", sm.GetSelectedShipData().GetShipName());
        }

        [UnityTest]
        public IEnumerator ShouldNotPurchaseShipIfNotEnoughCash()
        {
            sm.AddCash(10);
            sm.PurchaseShip("Cellex");
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreEqual("Rescate", sm.GetSelectedShipData().GetShipName());
        }

        [UnityTest]
        public IEnumerator ShouldGetCashCorrectly()
        {
            sm.AddCash(1000);
            sm.AddCash(3000);
            Assert.AreEqual(4000, sm.GetCash());
            PlayerPrefs.DeleteAll();
            Assert.AreEqual(0, sm.GetCash());
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
        }

        [UnityTest]
        public IEnumerator ShouldGetShip()
        {
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.NotNull(sm.GetShip("Cellex"));
        }
    }
}