using UnityEngine;
using System.Linq;
using Player;
using Utility;

namespace Manager
{
	/// <summary>
	/// Manages shop purchases and the ship details.
	/// </summary>
	public class ShopManager : MonoBehaviour
	{
		[SerializeField] private PlayerShip[] ships;
		[SerializeField] private PlayerShip selectedShip;

		public static ShopManager instance;

		private void Awake()
		{
			if (instance)
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
				instance = this;
			}
		}
	
		public void PurchaseShip(string name)
		{
			PlayerShip ship = GetShip(name);
			float cash = GetCash();
			float cost = ship.GetCost();

			if (cash >= cost && ship.IsUnlocked())
			{
				SetCash(cash - cost);
				selectedShip = ship;
				ship.SetUnlocked(true);
			}
			else
			{
				print ("Not enough wonga! You have: " + GetCash() + " and you need " + ship.GetCost());
			}
		}

		public PlayerShip GetShip(string name)
		{
			return ships.Where(ship => ship.GetShipName().Equals(name)).First();
		}

		public float GetCash()
		{
			return PlayerPrefs.GetFloat(Constants.CASH_KEY);
		}

		public void SetCash(float cash)
		{
			PlayerPrefs.SetFloat(Constants.CASH_KEY, cash);
		}
	}
}