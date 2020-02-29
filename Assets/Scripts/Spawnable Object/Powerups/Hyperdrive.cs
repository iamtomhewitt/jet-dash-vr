using UnityEngine;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Makes the player jump really high to avoid obstacles.
	/// <summary>
	public class Hyperdrive : Powerup
	{
		[SerializeField] private float distance;

		public override void ApplyPowerupEffect()
		{
			// TODO - alter the field of view of the camera to make it look like the hyperdrive is distorting
			// Do it with animation or with coding?
		}
	}
}