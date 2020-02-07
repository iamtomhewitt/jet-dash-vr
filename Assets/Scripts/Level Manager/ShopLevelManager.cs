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

		private int currentShipIndex = 0;

		private void Start()
		{
			ShipData ship = ShopManager.instance.GetShips()[currentShipIndex];
			SetShipDetails(ship);
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
			List<ShipData> ships = ShopManager.instance.GetShips();
			currentShipIndex = currentShipIndex > ships.Count ? 0 : currentShipIndex + 1;
			ShipData ship = ships[currentShipIndex];
			SetShipDetails(ship);
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void ShowPreviousShip()
		{
			List<ShipData> ships = ShopManager.instance.GetShips();
			currentShipIndex = currentShipIndex < 0 ? ships.Count : currentShipIndex - 1;
			ShipData ship = ships[currentShipIndex];
			SetShipDetails(ship);
		}

		private void SetShipDetails(ShipData shipData)
		{
			shipName.text 		= shipData.GetShipName();
			speed.text			= shipData.GetSpeed().ToString();
			turnSpeed.text		= shipData.GetTurningSpeed().ToString();
			cost.text			= shipData.GetCost().ToString();
			specialAbility.text = shipData.GetSpecialAbility().ToString();
		}
	}
}