using UnityEngine;
using System.Linq;
using Utility;
using Data;

namespace Manager
{
	/// <summary>
	/// Manages shop purchases and the ship details.
	/// </summary>
	public class ShopManager : MonoBehaviour
	{
		[SerializeField] private ShipData[] shipData;
		[SerializeField] private ShipData selectedShipData;

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
			ShipData shipData = GetShip(name);
			long cash = GetCash();
			long cost = shipData.GetCost();

			if (cash >= cost)
			{
				SetCash(cash - cost);
				selectedShipData = shipData;
			}
			else
			{
				print ("Not enough wonga! You have: " + GetCash() + " and you need " + shipData.GetCost());
			}
		}

		public ShipData GetShip(string name)
		{
			return shipData.Where(ship => ship.GetShipName().Equals(name)).First();
		}

		public ShipData GetSelectedShipData()
		{
			return selectedShipData;
		}

		public long GetCash()
		{
			return System.Convert.ToInt64(PlayerPrefs.GetString(Constants.CASH_KEY)); 
		}

		public void AddCash(long amount)
		{
			long currentCash = GetCash();
			long newTotal = currentCash + amount;
			SetCash(newTotal);
		}

		private void SetCash(long cash)
		{
			PlayerPrefs.SetString(Constants.CASH_KEY, "" + cash);
		}
	}
}