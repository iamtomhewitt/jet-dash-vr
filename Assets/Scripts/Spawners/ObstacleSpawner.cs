using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
	//TODO abstract out into Spawner Interface
    public class ObstacleSpawner : MonoBehaviour
    {
        public GameObject[] obstacles;
        public int totalObstacles;
        public ObstacleSize obstacleSize;
		public Material[] colours;

        [Space()]
        public Boundary boundary;
        public Boundary initialBoundary;

		private void Start()
        {
            InitialiseObstacles();
            InvokeRepeating("ReduceSpawnBoundary", 30f, 30f);
        }

		/// <summary>
		/// Instantiates all the obstacles that will be used in the game.
		/// </summary>
		private void InitialiseObstacles()
        {
            for (int i = 0; i < totalObstacles; i++)
            {
                // Work out a scale and how high the cube has to be so it sits nicely on top of the floor
                int scale = Random.Range(obstacleSize.minSize, obstacleSize.maxSize);
                float y = (scale / 2) + 0.5f;

                GameObject o = Instantiate(obstacles[Random.Range(0, obstacles.Length)], GenerateObstaclePosition(initialBoundary, y), Quaternion.identity) as GameObject;
                o.GetComponent<Obstacle>().Grow(1f, scale);
                o.transform.localScale = new Vector3(scale, scale, scale);
                o.transform.parent = GameObject.Find("Obstacles").transform;
				o.GetComponent<Renderer>().material = colours[Random.Range(0, colours.Length)];
            }
        }


		private void OnTriggerEnter(Collider other)
        {
			// An obstacle has moved past the player, respawn it in front.
            if (other.tag == "Obstacle")
            {
                other.transform.position = GenerateObstaclePosition(boundary, other.transform.position.y);
                other.GetComponent<Obstacle>().Grow(.5f, (int)other.transform.localScale.x);
            }
        }


		private Vector3 GenerateObstaclePosition(Boundary b, float y)
        {
            float x = Random.Range(b.xMin, b.xMax);
            float z = Random.Range(b.zMin, b.zMax);

            return new Vector3(transform.position.x + x, y, transform.position.z + z);
        }


        /// <summary>
        /// Reduces the spawn boundary to make the cubes appear tighter, making the game harder.
        /// </summary>
        private void ReduceSpawnBoundary()
        {
            if (boundary.zMin >= 150)
                boundary.zMin -= 50;

            if (boundary.zMax >= 400)
                boundary.zMax -= 50;
        }
    }

    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax;
        public float zMin, zMax;
    }

    [System.Serializable]
    public class ObstacleSize
    {
        public int minSize, maxSize;
    }
}
