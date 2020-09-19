using Player;
using SpawnableObject;
using UnityEngine;

namespace Utility
{
	/// <summary>
	/// Used to activate the animated obstacles. Used to have a collider on the animated obstacles themselves 
	/// but their radius would get so big that they would cause the powerups to constantly relocate, as they
	/// kept spawning in the vicinity of the sphere collider of the animated obstacles.
	/// </summary>
	public class ObstacleActivator : MonoBehaviour
	{
		private PlayerControl playerControl;
		private MatchTransformPosition transformMatcher;
		private float offsetPadding = 35f;

		private void Start()
		{
			playerControl = FindObjectOfType<PlayerControl>();
			transformMatcher = GetComponent<MatchTransformPosition>();
			InvokeRepeating("IncreasePosition", 0f, 1f);
		}

		private void IncreasePosition()
		{
			float newOffset = offsetPadding + playerControl.GetSpeed() * 1.5f;
			transformMatcher.SetZOffset(newOffset);
		}

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					if (other.GetComponentInParent<AnimatedObstacle>())
					{
						other.GetComponentInParent<AnimatedObstacle>().Animate();
					}
					break;

				default:
					// Nothing to do!
					break;
			}
		}
	}
}