using Achievements;
using Manager;
using Player;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Doubles the players points.
	/// <summary>
	public class DoublePoints : Powerup
	{
		public override void ApplyPowerupEffect()
		{
			FindObjectOfType<PlayerScore>().DoublePoints();
			this.GetPlayerHud().ShowNotification(GetColour(), "x2!");
			this.GetAudioManager().Play(SoundNames.DOUBLE_POINTS);
			AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_DOUBLE_POINTS);
		}
	}
}