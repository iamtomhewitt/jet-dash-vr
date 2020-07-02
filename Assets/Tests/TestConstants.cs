using UnityEngine;

namespace Tests
{
	public class TestConstants
	{
		public const float WAIT_TIME = 0.01f;

		public static GameObject GetResource(string path)
		{
			return Resources.Load<GameObject>("Tests/Prefabs/" + path);
		}
	}
}