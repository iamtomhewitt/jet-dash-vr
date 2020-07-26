using Achievements;
using Manager;
using Player;
using UnityEngine;
using Utility;

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
			FindObjectOfType<PlayerScore>().AddBonusPoints(bonusAmount);
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_BONUS_POINTS(bonusAmount));
			this.GetAudioManager().Play(SoundNames.BONUS_POINTS);
			AchievementManager.instance.UnlockAchievement(AchievementIds.FLY_THROUGH_BONUS_POINTS);
		}
	}
}