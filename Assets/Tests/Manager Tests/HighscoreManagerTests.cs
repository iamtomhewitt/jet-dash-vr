using Highscore;
using Manager;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using Utility;

namespace Tests
{
    public class HighscoreManagerTests
    {
        private AchievementManager am;
        private GameSettingsManager gs;
        private HighscoreManager hm;

        [SetUp]
        public void Setup()
        {
            PlayerPrefs.DeleteAll();
            am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
            gs = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Game Settings")).GetComponent<GameSettingsManager>();
            hm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Highscore Manager")).GetComponent<HighscoreManager>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(am.gameObject);
            Object.Destroy(gs.gameObject);
            Object.Destroy(hm.gameObject);
        }

        [UnityTest]
        public IEnumerator ShouldSaveNewHighscore()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HIGHSCORE, 50);
            hm.SaveLocalHighscore(100);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreEqual(100, hm.GetLocalHighscore());
        }

        [UnityTest]
        public IEnumerator ShouldSaveNewDistanceScore()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.DISTANCE, 150);
            hm.SaveDistanceHighscore(200);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreEqual(200, hm.GetBestDistance());
        }

        [UnityTest]
        public IEnumerator ShouldNotSaveHighscore()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HIGHSCORE, 100);
            hm.SaveLocalHighscore(50);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreNotEqual(50, hm.GetLocalHighscore());
        }

        [UnityTest]
        public IEnumerator ShouldNotSaveDistanceScore()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.DISTANCE, 100);
            hm.SaveDistanceHighscore(50);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreNotEqual(50, hm.GetBestDistance());
        }
    }
}