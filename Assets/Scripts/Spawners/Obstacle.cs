using System.Collections;
using UnityEngine;
using Utility;

namespace Spawner
{
	/// <summary>
	/// An obstacle that the player has to dodge.
	/// </summary>
	public class Obstacle : MonoBehaviour
	{
		private Transform player;

		private const float BEHIND_OFFSET = 30f;
		private const float RELOCATE_CHECK_TIME = 5f;

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;

			InvokeRepeating("RelocateIfBehindPlayer", RELOCATE_CHECK_TIME, RELOCATE_CHECK_TIME);
		}

		private void RelocateIfBehindPlayer()
		{
			if (transform.position.z < player.transform.position.z - BEHIND_OFFSET)
			{
				Relocate();
			}
		}

		/// <summary>
		/// Repositions and reanimates the obstacle.
		/// </summary>
		public void Relocate()
		{
			float x = SpawnableObjectRelocationBoundary.GetRandomX();
			float z = SpawnableObjectRelocationBoundary.GetRandomZ();

			this.transform.position = new Vector3(player.transform.position.x + x, this.transform.position.y, player.transform.position.z + z);

			Grow(Constants.OBSTACLE_GROW_SPEED, (int)transform.localScale.x);
		}

		/// <summary>
		/// Makes the obstacle grow over a certain time from scale 0.
		/// </summary>
		public void Grow(float duration, int scale)
		{
			StartCoroutine(GrowRoutine(duration, scale));
		}

		private IEnumerator GrowRoutine(float duration, int scale)
		{
			Vector3 originalScale = Vector3.zero;
			Vector3 targetScale = new Vector3(scale, scale, scale);

			float timer = 0f;

			do
			{
				transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
				timer += Time.deltaTime;
				yield return null;
			}
			while (timer <= duration);

			transform.localScale = targetScale;
		}		
	}
}
