using Achievements;
using Manager;
using Player;
using System.Collections;
using UnityEngine;
using Utility;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Makes the player invincible for a set amount of time.
	/// <summary>
	public class Invincibility : Powerup
	{
		[SerializeField] private float invincibilityTime = 5f;

		private Coroutine godModeRoutine;
		private const int FLICKER_COUNT = 3;
		private const float FLICKER_DURATION = 0.3f;

		public override void ApplyPowerupEffect()
		{
			if (godModeRoutine != null)
			{
				StopCoroutine(godModeRoutine);
			}
			godModeRoutine = StartCoroutine(ActivateGodMode());
			this.GetPlayerHud().ShowNotification(GetColour(), Ui.POWERUP_INVINCIBLE);
			this.GetAudioManager().Play(SoundNames.INVINCIBILITY_POINTS);
			AchievementManager.instance.UnlockAchievement(AchievementIds.BECOME_INVINCIBLE);
		}

		/// <summary>
		/// Makes the player invincible for a set amount of time, and shows the shield.
		/// </summary>
		private IEnumerator ActivateGodMode()
		{
			PlayerCollision playerCollision = FindObjectOfType<PlayerCollision>();
			playerCollision.SetInvincible(true);

			GameObject shield = playerCollision.GetShield();
			shield.SetActive(true);
			shield.GetComponent<Animator>().Play("On");

			yield return new WaitForSeconds(invincibilityTime);

			// Now flicker
			for (int i = 0; i < FLICKER_COUNT; i++)
			{
				shield.SetActive(false);
				yield return new WaitForSeconds(FLICKER_DURATION);
				shield.SetActive(true);
				yield return new WaitForSeconds(FLICKER_DURATION);
			}

			shield.SetActive(false);
			playerCollision.SetInvincible(false);
		}
	}
}