using UnityEngine;

namespace Data
{
	/// <summary>
	/// Data class of a ship that has a powerup associated with it.
	/// </summary>
	[CreateAssetMenu()]
	public class ShipWithPowerup : ShipData
	{
		[SerializeField] private SpecialAbilityWithPowerup powerup;

		public SpecialAbilityWithPowerup GetSpecialAbility()
		{
			return powerup;
		}
	}
}