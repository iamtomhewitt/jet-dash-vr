using UnityEngine;
using Utility;
using SpawnableObject;

namespace Spawner
{
	/// <summary>
	/// This GameObject has a large wall like collider attached to it. Think of it like a giant broomstick.
	/// When an obstacle hits the collider, it is repositioned. This means we are not continuously Destroying and Instantiating objects,
	/// we are reusing ones we have, saving on computing power on a mobile.
	/// </summary>
	public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] obstacles;
		[SerializeField] private ObstacleSize obstacleSize;
		[SerializeField] private Material[] colours;

		[SerializeField] private int totalObstacles;

		[Space()]
		[SerializeField] private SpawnBoundary boundary;
		[SerializeField] private SpawnBoundary initialBoundary;

		private const string OBSTACLE_PARENT = "Obstacles";
		private const float SPAWN_REDUCTION_TIME = 30f;

		private void Start()
        {
            InitialiseObstacles();
            InvokeRepeating("ReduceSpawnBoundary", SPAWN_REDUCTION_TIME, SPAWN_REDUCTION_TIME);
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
                o.GetComponent<Obstacle>().Grow(Constants.OBSTACLE_GROW_SPEED, scale);
				o.GetComponent<Renderer>().material = colours[Random.Range(0, colours.Length)];
                o.transform.localScale = new Vector3(scale, scale, scale);
                o.transform.parent = GameObject.Find(OBSTACLE_PARENT).transform;
            }
        }

		/// <summary>
		/// Generates a position for the obstacle. The y value is calculated beforehand to get the height right.
		/// </summary>
		private Vector3 GenerateObstaclePosition(SpawnBoundary b, float y)
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
			if (boundary.CanShrinkZMin())
			{
				boundary.ReduceZMin();
			}

			if (boundary.CanShrinkZMax())
			{
				boundary.ReduceZMax();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					other.GetComponent<Obstacle>().Relocate();
					break;

				default:
					// Nothing to do!
					break;
			}
		}
	}    

    [System.Serializable]
    public class ObstacleSize
    {
        public int minSize, maxSize;
    }
}
