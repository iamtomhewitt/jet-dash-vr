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
        [SerializeField] private float boostDuration = 2f;

        private bool camerasFinishedZooming = true;

        public override void ApplyPowerupEffect()
        {
            PlayerHud.instance.ShowNotification(GetColour(), "Hyperdrive!");
            StartCoroutine(ApplyPowerupEffectRoutine());
        }

        private IEnumerator ApplyPowerupEffectRoutine()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            PlayerControl playerControl = PlayerControl.instance;
            PlayerCollision playerCollision = PlayerCollision.instance;
            float originalSpeed = playerControl.GetSpeed();

            camerasFinishedZooming = false;
            playerControl.MaxSpeed();
            playerCollision.SetGodMode(true);

            foreach (Camera camera in cameras)
            {
                StartCoroutine(ZoomCamera(camera));
            }

            while (!camerasFinishedZooming)
            {
                yield return null;
            }

            playerControl.SetSpeed(originalSpeed);
            playerCollision.SetGodMode(false);
        }

        private IEnumerator ZoomCamera(Camera camera)
        {
            float originalFov = 60f;
            yield return StartCoroutine(IncreaseFov(camera, speedFieldOfView));
            yield return new WaitForSeconds(boostDuration);
            yield return StartCoroutine(DecreaseFov(camera, brakeFieldOfView));
            yield return StartCoroutine(IncreaseFov(camera, originalFov));
            camera.fieldOfView = originalFov;
            camerasFinishedZooming = true;
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
            float damp = 1.5f;
            do
            {
                camera.fieldOfView -= Time.deltaTime * zoomSpeed * damp;
                yield return null;
            }
            while (camera.fieldOfView > target);

            camera.fieldOfView = target;
        }
    }
}