using UnityEngine;
using Utility;

namespace SpawnableObject
{
	/// <summary>
	/// Applies a powerup when flown through.
	/// </summary>
	public abstract class Powerup : SpawnableObject
	{
		[SerializeField] private PowerupType powerupType;
		[SerializeField] private Color32 colour;

		/// <summary>
		/// For example, hitting the double points powerup doubles the players points.
		/// </summary>
		public abstract void ApplyPowerupEffect();

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					Debug.Log("A powerup has spawned inside an obstacle, moving...");
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

		public override void AfterRelocation()
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