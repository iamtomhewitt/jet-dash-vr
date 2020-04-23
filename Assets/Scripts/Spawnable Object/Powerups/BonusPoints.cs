using UnityEngine;
using Achievements;
using Player;
using Manager;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Gives the player some bonus points when flown through.!
	/// <summary>
	public class BonusPoints : Powerup
	{
		[SerializeField] private int bonusAmount = 500;

		public override void ApplyPowerupEffect()
		{
			PlayerScore.instance.AddBonusPoints(bonusAmount);
			PlayerHud.instance.ShowNotification(GetColour(), "+" + bonusAmount + "!");
			AudioManager.instance.Play(SoundNames.BONUS_POINTS);
			AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_BONUS_POINTS);
		}
	}
}