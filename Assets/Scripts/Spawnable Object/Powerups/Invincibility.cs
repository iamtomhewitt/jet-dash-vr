using UnityEngine;
using System.Collections;
using Achievements;
using Player;
using Manager;

namespace SpawnableObject.Powerups
{
	/// <summary>
	/// Makes the player invincible for a set amount of time.
	/// <summary>
	public class Invincibility : Powerup
	{
		[SerializeField] private float invincibilityTime = 5f;

		private Coroutine godModeRoutine;

		public override void ApplyPowerupEffect()
		{
			if (godModeRoutine != null)
			{
				StopCoroutine(godModeRoutine);
			}
			godModeRoutine = StartCoroutine(ActivateGodMode());
			AudioManager.instance.Play(SoundNames.INVINCIBILITY_POINTS);
			PlayerHud.instance.ShowNotification(GetColour(), "Invincible!");
			AchievementManager.instance.UnlockAchievement(AchievementIds.BECOME_INVINCIBLE);
		}

		/// <summary>
		/// Makes the player invincible for a set amount of time, and shows the shield.
		/// </summary>
		private IEnumerator ActivateGodMode()
		{
			GameObject shield = PlayerCollision.instance.GetShield();
			PlayerCollision.instance.SetInvincible(true);

			shield.SetActive(true);
			shield.GetComponent<Animator>().Play("On");

			yield return new WaitForSeconds(invincibilityTime);

			// Now flicker
			for (int i = 0; i < 3; i++)
			{
				shield.SetActive(false);
				yield return new WaitForSeconds(.3f);
				shield.SetActive(true);
				yield return new WaitForSeconds(.3f);
			}

			shield.SetActive(false);

			PlayerCollision.instance.SetInvincible(false);
		}
	}
}