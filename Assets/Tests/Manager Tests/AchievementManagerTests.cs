using Achievements;
using Manager;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests
{
    public class AchievementManagerTests
    {
        private AchievementManager achm;
		private AchievementNotificationManager achnm;
		private AudioManager am;

        private int id = 1;

        [SetUp]
        public void Setup()
        {
            achm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Manager")).GetComponent<AchievementManager>();
            achnm = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Achievement Notification Manager")).GetComponent<AchievementNotificationManager>();
            am = MonoBehaviour.Instantiate(TestConstants.GetResource("Managers/Audio Manager")).GetComponent<AudioManager>();
		}

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(achm);
            Object.Destroy(achnm);
            Object.Destroy(am);
        }

        [UnityTest]
        public IEnumerator ShouldGetAnAchievement()
        {
            ResetAchievement();
            Achievement a = achm.GetAchievement(id);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.NotNull(a);
        }

        [UnityTest]
        public IEnumerator ShouldUnlockAnAchievement()
        {
            ResetAchievement();
            achm.UnlockAchievement(id);
            Achievement a = achm.GetAchievement(id);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.True(a.unlocked);
        }

        [UnityTest]
        public IEnumerator ShouldProgressAchievement()
        {
            ResetAchievement();
            achm.ProgressAchievement(id, 5, 1);
            Achievement a = achm.GetAchievement(id);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.AreEqual(20f, a.progressPercentage);
        }

        [UnityTest]
        public IEnumerator ShouldNotifyIfUnlocked()
        {
            ResetAchievement();
            achm.UnlockAchievement(id);
            Achievement a = achm.GetAchievement(id);
            yield return new WaitForSeconds(TestConstants.WAIT_TIME);
            Assert.True(a.userShown);
        }

        private void ResetAchievement()
        {
            Achievement a = achm.GetAchievement(id);
            a.progressPercentage = 0f;
            a.unlocked = false;
            a.awardValue = 100;
            a.userShown = false;
        }
    }
}