using Achievements;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
	public class AchievementTests
	{
		[Test]
		public void ShouldInstantUnlockAchievement()
		{
			Achievement a = new Achievement(1, "test", "desc", 100);
			a.Unlock();
			Assert.AreEqual(100f, a.progressPercentage);
			Assert.True(a.unlocked);
		}

		[Test]
		public void ShouldUnlockAchievementAfterProgressing()
		{
			int value = 100;
			Achievement a = new Achievement(1, "test", "desc", value);
			a.Progress(value, 100);
			Assert.AreEqual(100f, a.progressPercentage);
			Assert.True(a.unlocked);
		}

		[Test]
		public void ShouldProgressAchievement()
		{
			int value = 100;
			Achievement a = new Achievement(1, "test", "desc", value);
			a.Progress(value, 50);
			Assert.AreEqual(50f, a.progressPercentage);
			Assert.False(a.unlocked);
		}

		[Test]
		public void ShouldNotBacktrackPercentageIfProgressingWithLowerValue()
		{
			int value = 100;
			Achievement a = new Achievement(1, "test", "desc", value);
			a.Progress(value, 50);
			a.Progress(value, 10);
			Assert.AreEqual(50f, a.progressPercentage);
			Assert.False(a.unlocked);
		}

		[Test]
		public void ShouldNotDoAnythingIfAlreadyUnlocked()
		{
			int value = 100;
			Achievement a = new Achievement(1, "test", "desc", value);
			a.Unlock();
			a.Progress(value, 10);
			Assert.AreEqual(100f, a.progressPercentage);
			Assert.True(a.unlocked);
		}
	}
}