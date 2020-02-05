using UnityEngine;
using System.Collections;
using Utility;

namespace SpawnableObject
{
	/// <summary>
	/// An obstacle that the player has to dodge.
	/// </summary>
	public class Obstacle : SpawnableObject
	{
		/// <summary>
		/// Makes the obstacle grow over a certain time from scale 0.
		/// </summary>
		public void Grow(float duration, int scale)
		{
			StartCoroutine(GrowRoutine(duration, scale));
		}

		private IEnumerator GrowRoutine(float duration, int scale)
		{
			Vector3 originalScale = new Vector3(0.1f, 0.1f, 0.1f);
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

		public override void PostRelocationMethod()
		{
			Grow(Constants.OBSTACLE_GROW_SPEED, (int)transform.localScale.x);
		}
	}
}