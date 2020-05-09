using UnityEngine;
using Player;
using Manager;
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
			PlayerHud.instance.ShowNotification(GetColour(), "Jump!");
			AudioManager.instance.Play(SoundNames.JUMP);
		}
	}
}