using Manager;
using UnityEngine;
using Utility;

namespace Highscores
{
	/// <summary>
	/// A GameObject at the distance highscore of the player. Matches the x position of the player so they dont lose it when moving.
	/// </summary>
	public class HighscoreMarker : MonoBehaviour
	{
		private Transform player;
		private float slerpSpeed = 1.75f;
		private int currentDistanceHighscore;
		private int zPosition = -100;

		private void Start()
		{
			player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
			currentDistanceHighscore = HighscoreManager.instance.GetBestDistance();

			zPosition = currentDistanceHighscore <= 500 ? zPosition : currentDistanceHighscore;
			transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
		}

		private void Update()
		{
			transform.position = Vector3.Slerp(transform.position, new Vector3(player.position.x, transform.position.y, zPosition), slerpSpeed * Time.deltaTime);
		}
	}
}