using UnityEngine;
using Utility;

namespace SpawnableObject
{
    /// <summary>
    /// An obstacle that the player has to dodge.
    /// </summary>
    public class AnimatedObstacle : SpawnableObject
    {
        [SerializeField] private Animator animator;

        private const string ANIMATION_EMTPY = "Empty";
        private const string ANIMATION_NAME = "Animating";

        public override void AfterRelocation()
        {
            animator.Play(ANIMATION_EMTPY);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case Tags.PLAYER:
                    print("should animate");
                    animator.Play(ANIMATION_NAME);
                    break;

                default:
                    // Nothing to do!
                    break;
            }
        }
    }
}