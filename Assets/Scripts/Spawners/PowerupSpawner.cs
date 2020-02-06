using UnityEngine;
using System.Collections.Generic;
using Utility;
using Manager;

namespace Spawner
{
	public class PowerupSpawner : MonoBehaviour
	{
		[SerializeField] private List<GameObject> powerups;
		[SerializeField] private float repeatRate;
		[SerializeField] private int maxPowerups;
		[SerializeField] private SpawnBoundary boundary;

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
			bool playerShipHasPowerup = ShopManager.instance.GetSelectedShipData().HasPowerup();

			if (playerShipHasPowerup)
			{
				GameObject additionalPowerup = ShopManager.instance.GetSelectedShipData().GetPowerup();
				powerups.Add(additionalPowerup);
			}

			for (int i = 0; i <= maxPowerups; i++)
			{
				int index = Random.Range(0, powerups.Count);

				GameObject powerup = powerups[index];

				float x = Random.Range(boundary.xMin, boundary.xMax);
				float z = Random.Range(boundary.zMin, boundary.zMax);

				Vector3 position = new Vector3(player.transform.position.x + x, 0f, player.transform.position.z + z);

				Instantiate(powerup, position, Quaternion.identity, parent);
			}
		}
	}
}
