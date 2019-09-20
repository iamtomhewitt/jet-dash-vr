using UnityEngine;
using Utility;

namespace Spawner
{
	public class Powerup : MonoBehaviour
	{
		[SerializeField] private PowerupType powerupType;
		[SerializeField] private Color32 colour;

		private Transform player;

		private const float CHECK_TIME = 5f;

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
			InvokeRepeating("RelocateIfBehindPlayer", CHECK_TIME, CHECK_TIME);
		}

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					print("A powerup has spawned inside an obstacle, moving...");
					transform.position -= Vector3.forward * 100f;
					break;

				default:
					// Nothing to do!
					break;
			}
		}

		private void RelocateIfBehindPlayer()
		{
			if (this.transform.position.z < player.transform.position.z)
			{
				Relocate();
				//print ("Powerup behind Player, respawning...");
			}
		}

		public void Relocate()
		{
			float x = SpawnableObjectRelocationBoundary.GetRandomX();
			float z = SpawnableObjectRelocationBoundary.GetRandomZ();

			this.transform.position = new Vector3(player.transform.position.x + x, this.transform.position.y, player.transform.position.z + z);
		}

		public PowerupType GetPowerupType()
		{
			return powerupType;
		}

		public Color32 GetColour()
		{
			return colour;
		}
	}

	public enum PowerupType
	{
		BonusPoints,
		DoublePoints,
		Invincibility
	};
}