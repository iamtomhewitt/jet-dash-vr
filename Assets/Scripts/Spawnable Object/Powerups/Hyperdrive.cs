using UnityEngine;
using System.Collections;
using Player;

namespace SpawnableObject.Powerups
{
    /// <summary>
    /// Makes the player have a burst of speed for a small amount of time.
    /// <summary>
    public class Hyperdrive : Powerup
    {
        [SerializeField] private float speedFieldOfView = 110f;
        [SerializeField] private float brakeFieldOfView = 45f;
        [SerializeField] private float zoomSpeed = 100f;

        public override void ApplyPowerupEffect()
        {
            PlayerHud.instance.ShowNotification(GetColour(), "Hyperdrive!");
            ZoomCameras();
        }

        private void ZoomCameras()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();

            foreach (Camera camera in cameras)
            {
                StartCoroutine(ZoomCamera(camera));
            }
        }

        private IEnumerator ZoomCamera(Camera camera)
        {
			float originalFov = 60f;
            yield return StartCoroutine(IncreaseFov(camera, speedFieldOfView));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(DecreaseFov(camera, brakeFieldOfView));
            yield return StartCoroutine(IncreaseFov(camera, originalFov));
            camera.fieldOfView = originalFov;
        }

        private IEnumerator IncreaseFov(Camera camera, float target)
        {
            do
            {
                camera.fieldOfView += Time.deltaTime * zoomSpeed;
                yield return null;
            }
            while (camera.fieldOfView < target);

            camera.fieldOfView = target;
        }

        private IEnumerator DecreaseFov(Camera camera, float target)
        {
            do
            {
                camera.fieldOfView -= Time.deltaTime * zoomSpeed;
                yield return null;
            }
            while (camera.fieldOfView > target);

            camera.fieldOfView = target;
        }
    }
}