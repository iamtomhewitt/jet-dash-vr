using UnityEngine;

namespace Utility
{
	public class Rotate : MonoBehaviour
	{
		public Vector3 speed;

		private void Update()
		{
			transform.Rotate(speed * Time.deltaTime);
		}
	}
}