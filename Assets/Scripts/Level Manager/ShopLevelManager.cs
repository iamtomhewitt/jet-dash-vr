﻿using Data;
using Manager;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace LevelManagers
{
	/// <summary>
	/// Handles everything on the Shop menu.
	/// </summary>
	public class ShopLevelManager : LevelManager
	{
		[SerializeField] private Image shipIcon;
		[SerializeField] private Text acceleration;
		[SerializeField] private Text cash;
		[SerializeField] private Text cost;
		[SerializeField] private Text description;
		[SerializeField] private Text shipName;
		[SerializeField] private Text specialAbility;
		[SerializeField] private Text specialAbilityDescription;
		[SerializeField] private Text speed;
		[SerializeField] private Text turnSpeed;

		private List<ShipData> ships;
		private AudioManager audioManager;
		private ShopManager shopManager;
		private int currentShipIndex = 0;

		private void Start()
		{
			audioManager = AudioManager.instance;
			shopManager = ShopManager.instance;

			ships = shopManager.GetShips();
			SetCashText();

			// Set the first ship to be shown the basic ship.
			for (int i = 0; i < ships.Count; i++)
			{
				if (ships[i].GetShipName().Equals(Constants.STARTING_SHIP))
				{
					currentShipIndex = i;
					break;
				}
			}

			SetShipDetails();
		}

		private void SetCashText()
		{
			cash.SetText(Ui.SHOP_SHIP_CASH(shopManager.GetCash()));
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void BuyShip()
		{
			ShipData ship = ships[currentShipIndex];
			shopManager.PurchaseShip(ship.GetShipName());
			SetShipDetails();
			SetCashText();
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
			ShipData shipData = ships[currentShipIndex];
			bool isUnlocked = shopManager.IsShipUnlocked(shipData);

			shipName.SetText(shipData.GetShipName());
			shipName.color = shipData.GetShipName().Equals(shopManager.GetSelectedShipData().GetShipName()) ? Color.green : Color.white;
			description.SetText(shipData.GetDescription());
			speed.SetText(Ui.SHOP_SHIP_SPEED(shipData.GetSpeed()));
			acceleration.SetText(Ui.SHOP_SHIP_ACCELERATION(shipData.GetAcceleration()));
			turnSpeed.SetText(Ui.SHOP_SHIP_TURN_SPEED(shipData.GetTurningSpeed()));
			cost.SetText(Ui.SHOP_SHIP_COST(shipData.GetCost()));
			specialAbility.SetText(shipData.GetSpecialAbility().ToString());
			specialAbilityDescription.SetText(shipData.GetSpecialAbilityDescription());
			shipIcon.sprite = shipData.GetSprite();
			shipIcon.color = isUnlocked ? Color.white : Color.black;
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void SelectShip()
		{
			ShipData shipData = ships[currentShipIndex];
			bool isUnlocked = shopManager.IsShipUnlocked(shipData);

			if (isUnlocked)
			{
				shopManager.SetSelectedShipData(shipData);
				shipName.color = Color.green;
				audioManager.Play(SoundNames.SHOP_SELECT_SHIP);
			}
			else
			{
				audioManager.Play(SoundNames.SHOP_REJECT_PURCHASE);
			}
		}
	}
}