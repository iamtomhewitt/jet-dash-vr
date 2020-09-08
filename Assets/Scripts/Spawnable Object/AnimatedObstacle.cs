using Player;
using UnityEngine;
using Utility;

namespace SpawnableObject
{
	public class AnimatedObstacle : SpawnableObject
	{
		[SerializeField] private Animator animator;

		private PlayerControl playerControl;
		private SphereCollider activationCollider;
		private float activationRadius = 20f;

		private const string ANIMATION_EMTPY = "Empty";
		private const string ANIMATION_NAME = "Animating";

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
			animator.Play(ANIMATION_EMTPY);
		}

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.PLAYER:
					animator.Play(ANIMATION_NAME);
					break;

				default:
					// Nothing to do!
					break;
			}
		}
	}
}