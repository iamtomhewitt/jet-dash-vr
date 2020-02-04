using UnityEngine;

namespace Player
{
	/// <summary>
	/// Details about the players ship, which are also displayed in the shop.
	/// </summary>
	[CreateAssetMenu()]
	public class PlayerShip : ScriptableObject
	{
		[SerializeField] private string shipName;
		[SerializeField] private SpecialAbility specialAbility;
		[SerializeField] private float speed;
		[SerializeField] private float speedIncreaseRate;
		[SerializeField] private float turnSpeed;
		[SerializeField] private float cost;

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
	}

	/// <summary>
	/// The names of the special abilities that each of the ships have.
	/// </summary>
	public enum SpecialAbility { Slugger, Speedster, Frogger, Hyperdrive, Diamond }
}