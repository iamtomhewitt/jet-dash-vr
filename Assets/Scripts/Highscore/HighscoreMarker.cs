using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// A GameObject at the distance highscore of the player. Matches the XX position of the player so they dont lost it when moving.
/// </summary>
public class HighscoreMarker : MonoBehaviour
{
	private Transform player;
	private int currentDistanceHighscore;
	private int zPosition = -100;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		currentDistanceHighscore = PlayerPrefs.GetInt(GameSettings.distanceKey);

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
