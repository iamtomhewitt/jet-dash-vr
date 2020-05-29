using Achievements;
using Manager;
using Player;
using Utility;

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
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_DOUBLE_POINTS);
			this.GetAudioManager().Play(SoundNames.DOUBLE_POINTS);
			AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_DOUBLE_POINTS);
		}
	}
}