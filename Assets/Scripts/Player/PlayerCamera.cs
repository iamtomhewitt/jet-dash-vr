using UnityEngine;
using System.Collections;

namespace Player
{
	/// <summary>
	/// A class for controlling events to do with the player camera.
	/// </summary>
	public class PlayerCamera : MonoBehaviour
	{
		[SerializeField] private float speedFieldOfView = 110f;
		[SerializeField] private float brakeFieldOfView = 45f;

		private float zoomSpeed;
		private bool camerasFinishedZooming = true;

		public static PlayerCamera instance;

		private void Awake()
		{
			instance = this;
		}

		public void ZoomCameras(float zoomSpeed, float duration)
		{
			this.zoomSpeed = zoomSpeed;
			camerasFinishedZooming = false;

			Camera[] cameras = FindObjectsOfType<Camera>();
			foreach (Camera camera in cameras)
			{
				StartCoroutine(ZoomCamera(camera, duration));
			}
		}

		private IEnumerator ZoomCamera(Camera camera, float duration)
		{
			float originalFov = 60f;
			yield return StartCoroutine(IncreaseFov(camera, speedFieldOfView));
			yield return new WaitForSeconds(duration);
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
	
		public bool HasCamerasFinishedZooming()
		{
			return camerasFinishedZooming;
		}
	}
}