using UnityEngine;

namespace Utility
{
    public class MatchTransformPosition : MonoBehaviour
    {
		[SerializeField] private string targetTag;
        [SerializeField] private bool trackX;
        [SerializeField] private bool trackY;
        [SerializeField] private bool trackZ;

        [ConditionalField("trackX")]
        [SerializeField] private float xOffset;
        [ConditionalField("trackY")]
        [SerializeField] private float yOffset;
        [ConditionalField("trackZ")]
        [SerializeField] private float zOffset;

        private Transform target;
        private float x = 0f, y = 0f, z = 0f;

		private void Start()
		{
			target = GameObject.FindWithTag(targetTag).transform;
		}

        private void Update()
        {
            if (target)
            {
                if (trackX)
                {
                    x = target.position.x;
                }
                if (trackY)
                {
                    y = target.position.y;
                }
                if (trackZ)
                {
                    z = target.position.z;
                }

                transform.SetPosition(new Vector3(x, y, z) + new Vector3(xOffset, yOffset, zOffset));
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

		public void SetZOffset(float offset)
		{
			this.zOffset = offset;
		}
    }
}
