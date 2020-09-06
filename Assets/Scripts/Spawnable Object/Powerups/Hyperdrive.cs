using Manager;
using Player;
using System.Collections;
using UnityEngine;
using Utility;

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
			PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
			PlayerCollision playerCollision = FindObjectOfType<PlayerCollision>();
			PlayerControl playerControl = FindObjectOfType<PlayerControl>();

			float originalSpeed = playerControl.GetSpeed();

			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_HYPERDRIVE);
			this.GetAudioManager().Play(SoundNames.HYPERDRIVE);

			playerControl.MaxSpeed();
			playerCollision.SetHyperdriveEnabled(true);
			playerCamera.ZoomCameras(cameraZoomSpeed, boostDuration);

			while (!playerCamera.HasCamerasFinishedZooming())
			{
				yield return null;
			}

			playerControl.SetSpeed(originalSpeed);
			playerControl.SetReachedMaxSpeed(false);
			playerCollision.SetHyperdriveEnabled(false);
		}
	}
}