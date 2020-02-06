using UnityEngine;
using Utility;

namespace SpawnableObject
{
	/// <summary>
	/// An objects that can be spawned in front of the player, such as powerups and obstacles
	/// </summary>
	public abstract class SpawnableObject : MonoBehaviour
	{
		[SerializeField] private float spawnOffset;

		private Transform player;
		private const float RELOCATE_CHECK_TIME = 5f;

		/// <summary>
		/// A method to be called once the object has been relocated (for example, Obstacles.Grow())
		/// </summary>
		public abstract void AfterRelocation();

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
			InvokeRepeating("RelocateIfBehindPlayer", RELOCATE_CHECK_TIME, RELOCATE_CHECK_TIME);
		}

		/// <summary>
		/// Relocates the object if it falls behind the player.
		/// </summary>
		private void RelocateIfBehindPlayer()
		{
			if (transform.position.z < player.transform.position.z - spawnOffset)
			{
				Relocate();
				AfterRelocation();
			}
		}

		/// <summary>
		/// Repositions the obstacle.
		/// </summary>
		public void Relocate()
		{
			float x = player.transform.position.x + SpawnableObjectRelocationBoundary.GetRandomX();
			float y = this.transform.position.y;
			float z = player.transform.position.z + SpawnableObjectRelocationBoundary.GetRandomZ();

			this.transform.position = new Vector3(x, y, z);
		}
	}
}