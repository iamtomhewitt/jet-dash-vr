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
		[SerializeField] private float turnSpeed;
		[SerializeField] private float cost;

		public string GetShipName()
		{
			return shipName;
		}
	}

	/// <summary>
	/// The names of the special abilities that each of the ships have.
	/// </summary>
	public enum SpecialAbility { Slugger, Speedster, Frogger, Hyperdrive, Diamond }
}