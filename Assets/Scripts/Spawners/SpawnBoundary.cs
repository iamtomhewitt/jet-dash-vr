namespace Spawner
{
	[System.Serializable]
	public class SpawnBoundary
	{
		public float xMin, xMax;
		public float zMin, zMax;

		private float zMinLimit = 150f;
		private float zMaxLimit = 400f;

		private float reductionStep = 50f;

		public bool CanShrinkZMin()
		{
			return zMin >= zMinLimit;
		}

		public bool CanShrinkZMax()
		{
			return zMax >= zMaxLimit;
		}

		public void ReduceZMin()
		{
			zMin -= reductionStep;
		}

		public void ReduceZMax()
		{
			zMax -= reductionStep;
		}
	}
}
