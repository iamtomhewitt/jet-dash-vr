using UnityEngine;

namespace Utility
{
	public static class Extensions
	{
		public static void RotateOnZAxisByAccelerometer(this Transform t, bool inputInDeadzone, float limit)
		{
			float z = 0f;
			if (!inputInDeadzone)
			{
				z = Input.acceleration.x * 30f;
				z = Mathf.Clamp(z, -limit, limit);
			}
			else
			{
				z = 0f;
			}
			t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, -z);
		}

		public static void SetYPosition(this Transform t, float y)
		{
			t.position = new Vector3(t.position.x, y, t.position.z);
		}
	}
}