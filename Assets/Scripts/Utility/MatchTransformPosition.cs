using UnityEngine;

namespace Utility
{
	public class MatchTransformPosition : MonoBehaviour
	{
		public Transform target;
		public Vector3 offset;

		private void Update()
		{
			transform.position = target.position + offset;
		}
	}
}
