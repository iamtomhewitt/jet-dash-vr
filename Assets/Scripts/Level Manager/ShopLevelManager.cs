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
		[SerializeField] private Text description;
		[SerializeField] private Text speed;
		[SerializeField] private Text turnSpeed;
		[SerializeField] private Text cost;
		[SerializeField] private Text specialAbility;
		[SerializeField] private Text specialAbilityDescription;
		[SerializeField] private Image shipIcon;

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
		public void BuyShip()
		{
			ShipData ship = ships[currentShipIndex];
			ShopManager.instance.PurchaseShip(ship.GetShipName());
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
			ShipData shipData 				= ships[currentShipIndex];
			shipName.text 					= shipData.GetShipName();
			description.text				= shipData.GetDescription();
			speed.text						= shipData.GetSpeed().ToString();
			turnSpeed.text					= shipData.GetTurningSpeed().ToString();
			cost.text						= "COST: " + shipData.GetCost().ToString() + "P";
			specialAbility.text 			= shipData.GetSpecialAbility().ToString();
			specialAbilityDescription.text 	= shipData.GetSpecialAbilityDescription();
			shipIcon.sprite					= shipData.GetSprite();
		}
	}
}