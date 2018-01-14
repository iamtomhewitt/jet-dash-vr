using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour 
{
	private Transform player;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;	

		InvokeRepeating ("CheckIfBehindPlayer", 20f, 20f);
	}

	void CheckIfBehindPlayer()
	{
		if (this.transform.position.z < player.transform.position.z) 
		{
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, player.transform.position.z + 100f);
			print ("Cube behind Player, respawning...");
		}
	}
}
