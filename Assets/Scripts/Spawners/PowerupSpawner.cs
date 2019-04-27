using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class PowerupSpawner : MonoBehaviour
    {
        public GameObject[] powerups;
        public float repeatRate;
        public int maxPowerups;
        public Boundary boundary;

        private Transform player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

            for (int i = 0; i <= maxPowerups; i++)
            {
                SpawnPowerup();
            }
        }


		/// <summary>
		/// Spawns a powerup at a random position on the map, in front of the player.
		/// </summary>
        private void SpawnPowerup()
        {
            int i = Random.Range(0, powerups.Length);

            GameObject powerup = powerups[i];

            float x = Random.Range(boundary.xMin, boundary.xMax);
            float z = Random.Range(boundary.zMin, boundary.zMax);
            Vector3 position = new Vector3(player.transform.position.x + x, 0f, player.transform.position.z + z);

            Instantiate(powerup, position, Quaternion.identity);
        }
    }
}
