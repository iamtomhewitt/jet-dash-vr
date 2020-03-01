using UnityEngine;
using System.Collections;
using Player;

namespace SpawnableObject.Powerups
{
    /// <summary>
    /// Makes the player jump really high to avoid obstacles.
    /// <summary>
    public class Hyperdrive : Powerup
    {
        [SerializeField] private float speedFieldOfView = 110f;
        [SerializeField] private float brakeFieldOfView = 45f;
        [SerializeField] private float zoomSpeed = 100f;

        public override void ApplyPowerupEffect()
        {
            PlayerHud.instance.ShowNotification(GetColour(), "Hyperdrive!");
            StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {           
            Camera camera = Camera.main;
			float originalFov = 60f;

            yield return StartCoroutine(IncreaseFov(camera, speedFieldOfView, zoomSpeed));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(DecreaseFov(camera, brakeFieldOfView, zoomSpeed));
            yield return StartCoroutine(IncreaseFov(camera, originalFov, zoomSpeed));

            camera.fieldOfView = originalFov;
        }

		private IEnumerator IncreaseFov(Camera camera, float target, float speed)
		{
			do
            {
                camera.fieldOfView += Time.deltaTime * speed;
                yield return null;
            }
            while (camera.fieldOfView < target);

            camera.fieldOfView = target;
		}

		private IEnumerator DecreaseFov(Camera camera, float target, float speed)
		{
			do
            {
                camera.fieldOfView -= Time.deltaTime * speed;
                yield return null;
            }
            while (camera.fieldOfView > target);

            camera.fieldOfView = target;
		}
    }
}