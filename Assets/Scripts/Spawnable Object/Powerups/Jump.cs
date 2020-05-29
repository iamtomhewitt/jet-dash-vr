using Manager;
using UnityEngine;
using Utility;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Makes the player jump really high to avoid obstacles.
	/// <summary>
	public class Jump : Powerup
	{
		[SerializeField] private float jumpPower;

		public override void ApplyPowerupEffect()
		{
			Rigidbody rb = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Rigidbody>();
			rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_JUMP);
			this.GetAudioManager().Play(SoundNames.JUMP);
		}
	}
}