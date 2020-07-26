using UnityEngine;

namespace Tests
{
	public class TestConstants
	{
		public const float WAIT_TIME = 0.05f;

		public static GameObject GetResource(string path)
		{
			return Resources.Load<GameObject>("Tests/" + path);
		}
	}
}