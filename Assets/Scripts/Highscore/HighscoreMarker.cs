using Manager;
using UnityEngine;
using Utility;

namespace Highscore
{
	/// <summary>
	/// A GameObject at the distance highscore of the player. Matches the x position of the player so they dont lose it when moving.
	/// </summary>
	public class HighscoreMarker : MonoBehaviour
	{
		private Transform player;

		private int currentDistanceHighscore;
		private int zPosition = -100;

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
			currentDistanceHighscore = HighscoreManager.instance.GetBestDistance();

			if (currentDistanceHighscore <= 500)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
			}
			else
			{
				zPosition = currentDistanceHighscore;
				transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
			}
		}

		private void Update()
		{
			transform.position = new Vector3(player.position.x, transform.position.y, zPosition);
		}
	}
}