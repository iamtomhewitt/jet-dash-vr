using System.Collections;
using UnityEngine;

namespace SpawnableObject
{
	/// <summary>
	/// An obstacle that the player has to dodge.
	/// </summary>
	public class StationaryObstacle : SpawnableObject
	{
		private const float GROW_SPEED = 0.75f;

		/// <summary>
		/// Makes the obstacle grow over a certain time from scale 0.
		/// </summary>
		public void Grow(int scale)
		{
			StartCoroutine(GrowRoutine(GROW_SPEED, scale));
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

		public override void AfterRelocation()
		{
			Grow((int)transform.localScale.x);
		}
	}
}