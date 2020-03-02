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
		[SerializeField] private float cameraZoomSpeed = 100f;
		[SerializeField] private float boostDuration = 2f;

		public override void ApplyPowerupEffect()
		{
			StartCoroutine(ApplyPowerupEffectRoutine());
		}

		private IEnumerator ApplyPowerupEffectRoutine()
		{
			PlayerControl playerControl = PlayerControl.instance;
			PlayerCollision playerCollision = PlayerCollision.instance;
			PlayerCamera playerCamera = PlayerCamera.instance;
			float originalSpeed = playerControl.GetSpeed();

			PlayerHud.instance.ShowNotification(GetColour(), "Hyperdrive!");

			playerControl.MaxSpeed();
			playerCollision.SetHyperdriveEnabled(true);
			playerCamera.ZoomCameras(cameraZoomSpeed, boostDuration);

			while (!playerCamera.HasCamerasFinishedZooming())
			{
				yield return null;
			}

			playerControl.SetSpeed(originalSpeed);
			playerCollision.SetHyperdriveEnabled(false);
		}
	}
}