using Manager;
using Player;
using UnityEngine;
using Utility;

namespace SpawnableObject.Powerups
{
	public class SlowDown : Powerup
	{
		[SerializeField] private int speedDecrease = 20;

		public override void ApplyPowerupEffect()
		{
			PlayerControl pc = FindObjectOfType<PlayerControl>();
			float newSpeed = pc.GetSpeed() - speedDecrease;
			if (newSpeed >= 0f)
			{
				pc.SetSpeed(newSpeed);
			}

			this.GetPlayerHud().SetSpeedText(pc.GetSpeed());
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_SLOW_DOWN);
			this.GetAudioManager().Play(SoundNames.SLOW_DOWN);
		}
	}
}