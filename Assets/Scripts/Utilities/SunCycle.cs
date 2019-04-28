using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	/// <summary>
	/// Rotates to give a sun moving across the hoirzon, which speeds up at night for a shorter time span.
	/// </summary>
	public class SunCycle : MonoBehaviour
	{
		public Vector3 daySpeed;
		public Vector3 nightSpeed;

		private Vector3 speed;

		private float angle;

		void Update()
		{
			transform.Rotate(speed * Time.deltaTime, Space.World);

			angle = transform.localEulerAngles.z;

			if (angle < 270 && angle > 90)
				speed = nightSpeed;
			else
				speed = daySpeed;
		}
	}
}
