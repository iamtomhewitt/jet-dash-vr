using Player;
using UnityEngine;
using Utility;

namespace SpawnableObject
{
	public class AnimatedObstacle : SpawnableObject
	{
		[SerializeField] private Animator animator;
		[SerializeField] private AnimatedObstacleType type;

		private PlayerControl playerControl;
		private SphereCollider activationCollider;
		private float activationRadius = 20f;

		private const string ANIMATION_EMPTY = "Empty";

		public override void Start()
		{
			base.Start();
			activationCollider = GetComponent<SphereCollider>();
			playerControl = FindObjectOfType<PlayerControl>();
			InvokeRepeating("IncreaseRadius", 0f, 1f);
		}

		private void IncreaseRadius()
		{
			float newRadius = (playerControl.GetSpeed() * 1.5f) + activationRadius;
			activationCollider.radius = newRadius;
		}

		public override void AfterRelocation()
		{
			animator.Play(ANIMATION_EMPTY);
		}

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.PLAYER:
					animator.Play(type.ToString());
					break;

				default:
					// Nothing to do!
					break;
			}
		}

		public enum AnimatedObstacleType
		{
			Rising, Closing
		}
	}
}