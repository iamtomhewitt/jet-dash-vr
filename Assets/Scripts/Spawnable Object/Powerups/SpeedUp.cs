using Manager;
using Player;
using UnityEngine;
using Utility;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Gives the player some bonus points when flown through.!
	/// <summary>
	public class SpeedUp : Powerup
	{
		public override void ApplyPowerupEffect()
		{
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_SPEED_UP);
			this.GetAudioManager().Play(SoundNames.SPEED_UP);
		}
	}
}