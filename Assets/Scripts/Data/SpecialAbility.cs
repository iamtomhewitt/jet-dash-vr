using UnityEngine;

namespace Data 
{
	/// <summary>
	/// The names of the special abilities that each of the ships have.
	/// </summary>
	public enum SpecialAbility { Slugger, Speedster, Diamond }
	public enum PowerupAbility { Frogger, Hyperdrive }

	[System.Serializable]
	public struct SpecialAbilityWithPowerup
	{
		[SerializeField] private PowerupAbility ability;
		[SerializeField] private GameObject powerup;
	}
}