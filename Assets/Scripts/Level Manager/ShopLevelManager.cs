using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Manager;
using Data;

namespace LevelManagers
{
	/// <summary>
	/// Handles everything on the Shop menu.
	/// </summary>
	public class ShopLevelManager : LevelManager
	{
		[SerializeField] private Text shipName;
		[SerializeField] private Text speed;
		[SerializeField] private Text turnSpeed;
		[SerializeField] private Text cost;
		[SerializeField] private Text specialAbility;

		private List<ShipData> ships;
		private int currentShipIndex = 0;

		private void Start()
		{
			ships = ShopManager.instance.GetShips();
			SetShipDetails();
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void BuyShip(string name)
		{
			ShopManager.instance.PurchaseShip(name);
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void ShowNextShip()
		{
			currentShipIndex = ++currentShipIndex > ships.Count - 1 ? 0 : currentShipIndex;
			SetShipDetails();
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void ShowPreviousShip()
		{
			currentShipIndex = --currentShipIndex < 0 ? ships.Count - 1 : currentShipIndex;
			SetShipDetails();
		}

		private void SetShipDetails()
		{
			ShipData shipData 	= ships[currentShipIndex];
			shipName.text 		= shipData.GetShipName();
			speed.text			= shipData.GetSpeed().ToString();
			turnSpeed.text		= shipData.GetTurningSpeed().ToString();
			cost.text			= shipData.GetCost().ToString();
			specialAbility.text = shipData.GetSpecialAbility().ToString();
		}
	}
}