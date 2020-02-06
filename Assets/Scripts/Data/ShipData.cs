using UnityEngine;

namespace Data
{
	/// <summary>
	/// Data class to hold information about different kinds of ships. This is a template data class and values should not be modified.
	/// </summary>
	[CreateAssetMenu()]
	public class ShipData : ScriptableObject
	{
		[SerializeField] private readonly string shipName;
		[SerializeField] private readonly SpecialAbility specialAbility;
		[SerializeField] private readonly float speed;
		[SerializeField] private readonly float speedIncreaseRate;
		[SerializeField] private readonly float turnSpeed;
		[SerializeField] private readonly long cost;

		public string GetShipName()
		{
			return shipName;
		}

		public float GetSpeed()
		{
			return speed;
		}

		/// <summary>
		/// The rate at which the speed is increased in seconds.
		/// </summary>
		public float GetSpeedIncreaseRate()
		{
			return speedIncreaseRate;
		}

		public float GetTurningSpeed()
		{
			return turnSpeed;
		}

		public long GetCost()
		{
			return cost;
		}
	}

	/// <summary>
	/// The names of the special abilities that each of the ships have.
	/// </summary>
	public enum SpecialAbility { Slugger, Speedster, Frogger, Hyperdrive, Diamond }
}