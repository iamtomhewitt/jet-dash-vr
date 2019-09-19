using UnityEngine;

namespace Spawner
{
	/// <summary>
	/// A boundary for which spawnable objects use to relocate themselves.
	/// </summary>
	public class SpawnableObjectRelocationBoundary
	{
		public const float minX = -300f;
		public const float maxX = 300f;
		public const float minZ = 600f;
		public const float maxZ = 2000f;

		public static float GetRandomX()
		{
			return Random.Range(minX, maxX);
		}

		public static float GetRandomZ()
		{
			return Random.Range(minZ, maxZ);
		}
	}
}
