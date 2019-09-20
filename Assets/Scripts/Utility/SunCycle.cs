using UnityEngine;

namespace Utility
{
	/// <summary>
	/// Rotates to give a sun moving across the hoirzon, which speeds up at night for a shorter time span.
	/// </summary>
	public class SunCycle : MonoBehaviour
	{
		[SerializeField] private Vector3 daySpeed;
		[SerializeField] private Vector3 nightSpeed;

		private Vector3 speed;

		private float angle;

		private void Update()
		{
			transform.Rotate(speed * Time.deltaTime, Space.World);

			angle = transform.localEulerAngles.z;

			if (angle < 270 && angle > 90)
			{
				speed = nightSpeed;
			}
			else
			{
				speed = daySpeed;
			}
		}
	}
}
