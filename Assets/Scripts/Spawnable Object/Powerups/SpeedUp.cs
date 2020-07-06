using Manager;
using Player;
using UnityEngine;
using Utility;

namespace SpawnableObject.Powerups
{
	public class SpeedUp : Powerup
	{
		[SerializeField] private int speedIncrease = 20;

		public override void ApplyPowerupEffect()
		{
			PlayerControl pc = FindObjectOfType<PlayerControl>();
			pc.SetSpeed(pc.GetSpeed() + speedIncrease);
			this.GetPlayerHud().SetSpeedText(pc.GetSpeed());
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_SPEED_UP);
			this.GetAudioManager().Play(SoundNames.SPEED_UP);
		}
	}
}