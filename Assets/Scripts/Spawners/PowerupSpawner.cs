using Manager;
using SpawnableObject;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Spawner
{
	public class PowerupSpawner : MonoBehaviour
	{
		[SerializeField] private List<Powerup> powerups;
		[SerializeField] private SpawnBoundary boundary;
		[SerializeField] private float repeatRate;
		[SerializeField] private int maxPowerups;
		[SerializeField] private int maxDoublePointsPowerups = 2;
		[SerializeField] private int maxInvincibilityPowerups = 4;
		[SerializeField] private int maxBonusPointsPowerups = 4;
		[SerializeField] private int maxSpeedUpPowerups = 2;
		[SerializeField] private int maxShipPowerups = 3;

		private List<Powerup> powerupsInPlay = new List<Powerup>();
		private Transform player;
		private Transform parent;

		private const string POWERUP_PARENT = "Powerups";

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
			parent = GameObject.Find(POWERUP_PARENT).transform;
			InitialisePowerups();
		}

		/// <summary>
		/// Spawns a powerup at a random position on the map, in front of the player.
		/// </summary>
		private void InitialisePowerups()
		{
			ShopManager shopManager = ShopManager.instance;
			bool playerShipHasPowerup = shopManager.GetSelectedShipData().HasPowerup();

			if (playerShipHasPowerup)
			{
				GameObject additionalPowerup = shopManager.GetSelectedShipData().GetPowerup();
				powerups.Add(additionalPowerup.GetComponent<Powerup>());
			}

			for (int i = 0; i <= maxPowerups; i++)
			{
				bool canSpawnPowerup = true;
				int index = Random.Range(0, powerups.Count);

				Powerup powerup = powerups[index];

				int totalBonusPointPowerups = GetTotalPowerupsByType(PowerupType.BonusPoints);
				int totalDoublePointPowerups = GetTotalPowerupsByType(PowerupType.DoublePoints);
				int totalInvincibilityPowerups = GetTotalPowerupsByType(PowerupType.Invincibility);
				int totalSpeedUpPowerups = GetTotalPowerupsByType(PowerupType.SpeedUp);
				int totalShipPowerups = GetTotalPowerupsByType(PowerupType.Hyperdrive) + GetTotalPowerupsByType(PowerupType.Jump);

				switch (powerup.GetPowerupType())
				{
					case PowerupType.BonusPoints:
						canSpawnPowerup = totalBonusPointPowerups < maxBonusPointsPowerups;
						break;
					case PowerupType.DoublePoints:
						canSpawnPowerup = totalDoublePointPowerups < maxDoublePointsPowerups;
						break;
					case PowerupType.Invincibility:
						canSpawnPowerup = totalInvincibilityPowerups < maxInvincibilityPowerups;
						break;
					case PowerupType.SpeedUp:
						canSpawnPowerup = totalSpeedUpPowerups < maxSpeedUpPowerups;
						break;
					case PowerupType.Hyperdrive:
						canSpawnPowerup = totalShipPowerups < maxShipPowerups;
						break;
					case PowerupType.Jump:
						canSpawnPowerup = totalShipPowerups < maxShipPowerups;
						break;
				}

				if (canSpawnPowerup)
				{
					SpawnPowerup(powerup);
				}
			}

			// There should be at least be two ship powerups, check if there is
			if (playerShipHasPowerup)
			{
				int shipPowerups = GetTotalPowerupsByType(PowerupType.Hyperdrive) + GetTotalPowerupsByType(PowerupType.Jump);
				while (shipPowerups < 2)
				{
					Powerup additionalPowerup = shopManager.GetSelectedShipData().GetPowerup().GetComponent<Powerup>();
					SpawnPowerup(additionalPowerup);
					shipPowerups++;
				}
			}
		}

		private void SpawnPowerup(Powerup powerup)
		{
			float x = Random.Range(boundary.xMin, boundary.xMax);
			float z = Random.Range(boundary.zMin, boundary.zMax);

			Vector3 position = new Vector3(player.transform.position.x + x, 0f, player.transform.position.z + z);

			Instantiate(powerup.gameObject, position, Quaternion.identity, parent);
			powerupsInPlay.Add(powerup);
		}

		private int GetTotalPowerupsByType(PowerupType type)
		{
			return powerupsInPlay.Where(p => p.GetPowerupType().Equals(type)).Count();
		}
	}
}
