using UnityEngine;

namespace Data
{
	/// <summary>
	/// Data class of a ship that has a special ability associated with it.
	/// </summary>
	[CreateAssetMenu()]
	public class ShipWithSpecialAbility : ShipData
	{
		[SerializeField] private SpecialAbility ability;

		public SpecialAbility GetSpecialAbility()
		{
			return ability;
		}
	}
}