using UnityEngine;

namespace Data
{
	/// <summary>
	/// Data class to hold information about different kinds of ships. This is a template data class and values should not be modified.
	/// </summary>
	public abstract class ShipData : ScriptableObject
	{
		[SerializeField] private string shipName;
		[SerializeField] private float speed;
		[SerializeField] private float speedIncreaseRate;
		[SerializeField] private float turnSpeed;
		[SerializeField] private long cost;

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
}