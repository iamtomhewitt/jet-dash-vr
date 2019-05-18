using Manager;
using Spawner;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Player
{
	/// <summary>
	/// Holds all the methods for when the Player collides with a powerup.
	/// </summary>
	public class PlayerPowerupManager : MonoBehaviour
	{
		public GameObject shield;

		public bool godMode;

		public static PlayerPowerupManager instance;

		private void Awake()
		{
			instance = this;
		}

		public void BonusPoints(Powerup powerup)
		{
			PlayerScore.instance.AddBonusPoints(500);
			AudioManager.instance.Play("Powerup Bonus Points");
			HUD.instance.ShowNotification(powerup.color, "+500!");
		}

		public void DoublePoints(Powerup powerup)
		{
			PlayerScore.instance.DoublePoints();
			AudioManager.instance.Play("Powerup Double Points");
			HUD.instance.ShowNotification(powerup.color, "x2!");
		}

		public void Invincibility(Powerup powerup)
		{
			StopAllCoroutines();
			StartCoroutine(GodMode(5f));
			AudioManager.instance.Play("Powerup Invincibility");
			HUD.instance.ShowNotification(powerup.color, "Invincible!");
		}
		
		/// <summary>
		/// Makes the player invincible for a set amount of time, and shows the shield.
		/// </summary>
		private IEnumerator GodMode(float duration)
		{
			godMode = true;

			shield.SetActive(true);
			shield.GetComponent<Animator>().Play("On");

			yield return new WaitForSeconds(duration);

			// Now flicker
			for (int i = 0; i < 3; i++)
			{
				shield.SetActive(false);
				yield return new WaitForSeconds(.3f);
				shield.SetActive(true);
				yield return new WaitForSeconds(.3f);
			}

			shield.SetActive(false);

			godMode = false;
		}
	}
}
