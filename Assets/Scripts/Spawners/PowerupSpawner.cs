using UnityEngine;

namespace Spawner
{
	public class PowerupSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject[] powerups;
		[SerializeField] private float repeatRate;
		[SerializeField] private int maxPowerups;
		[SerializeField] private SpawnBoundary boundary;

		private Transform player;

		private const string POWERUP_PARENT = "Powerups";

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;

			InitialisePowerups();
		}

		/// <summary>
		/// Spawns a powerup at a random position on the map, in front of the player.
		/// </summary>
		private void InitialisePowerups()
		{
			for (int i = 0; i <= maxPowerups; i++)
			{
				int index = Random.Range(0, powerups.Length);

				GameObject powerup = powerups[index];

				float x = Random.Range(boundary.xMin, boundary.xMax);
				float z = Random.Range(boundary.zMin, boundary.zMax);

				Vector3 position = new Vector3(player.transform.position.x + x, 0f, player.transform.position.z + z);

				GameObject p = Instantiate(powerup, position, Quaternion.identity) as GameObject;
				p.transform.parent = GameObject.Find(POWERUP_PARENT).transform;
			}
		}
	}
}
