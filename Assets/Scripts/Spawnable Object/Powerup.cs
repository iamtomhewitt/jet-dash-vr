using UnityEngine;
using Utility;

namespace SpawnableObject
{
	/// <summary>
	/// Applies a powerup when flow through.
	/// </summary>
	public class Powerup : SpawnableObject
	{
		[SerializeField] private PowerupType powerupType;
		[SerializeField] private Color32 colour;

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					print("A powerup has spawned inside an obstacle, moving...");
					transform.position -= Vector3.forward * 100f;
					break;

				default:
					// Nothing to do!
					break;
			}
		}

		public PowerupType GetPowerupType()
		{
			return powerupType;
		}

		public Color32 GetColour()
		{
			return colour;
		}

		public override void PostRelocationMethod()
		{
			// Nothing to do!
		}
	}

	public enum PowerupType
	{
		BonusPoints,
		DoublePoints,
		Invincibility
	};
}