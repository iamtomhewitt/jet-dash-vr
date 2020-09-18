using UnityEngine;

namespace SpawnableObject
{
	public class AnimatedObstacle : SpawnableObject
	{
		[SerializeField] private Animator animator;
		[SerializeField] private AnimatedObstacleType type;

		private const string ANIMATION_EMPTY = "Empty";

		public override void Start()
		{
			base.Start();
		}

		public override void AfterRelocation()
		{
			animator.Play(ANIMATION_EMPTY);
		}

		public void Animate()
		{
			animator.Play(type.ToString());
		}

		public enum AnimatedObstacleType
		{
			Popup, Closing, Rising
		}
	}
}