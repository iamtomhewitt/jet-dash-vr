﻿using UnityEngine;
using Utility;

namespace Data
{
	/// <summary>
	/// Data class to hold information about different kinds of ships. This is a template data class and values should not be modified.
	/// </summary>
	public class ShipData : ScriptableObject
	{
		[SerializeField] private string shipName;
		[SerializeField] private string description;
		[SerializeField] private float speed;
		[SerializeField] private float speedIncreaseRate;
		[SerializeField] private float turnSpeed;
		[SerializeField] private long cost;
		[SerializeField] private SpecialAbility ability;
		[SerializeField] private Sprite sprite;
		[SerializeField] private bool hasPowerup;
		
		[ConditionalField("hasPowerup")]
		[SerializeField] private GameObject powerup;

		public string GetShipName()
		{
			return shipName;
		}

		public string GetDescription()
		{
			return description;
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

		public bool HasPowerup()
		{
			return hasPowerup;
		}

		public GameObject GetPowerup()
		{
			return powerup;
		}

		public SpecialAbility GetSpecialAbility()
		{
			return ability;
		}

		public Sprite GetSprite()
		{
			return sprite;
		}

		public override string ToString()
		{
			return "Name: " + shipName +
					"\nSpeed: " + speed +
					"\nSpeed Increase Rate: " + speedIncreaseRate +
					"\nTurn Speed: " + turnSpeed +
					"\nCost: " + cost +
					"\nSpecial Ability: " + ability.ToString();
		}
	}

	/// <summary>
	/// The names of the special abilities that each of the ships have.
	/// </summary>
	public enum SpecialAbility { Slugger, Speedster, Diamond, Frogger, Hyperdrive }
}