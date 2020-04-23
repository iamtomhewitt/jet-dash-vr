using Achievements;
using Player;
using Manager;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Doubles the players points.
	/// <summary>
	public class DoublePoints : Powerup
	{
		public override void ApplyPowerupEffect()
		{
			PlayerScore.instance.DoublePoints();
			PlayerHud.instance.ShowNotification(GetColour(), "x2!");
			AudioManager.instance.Play(SoundNames.DOUBLE_POINTS);
			AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_DOUBLE_POINTS);
		}
	}
}