using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour 
{
    public 	PowerupType powerupType;
	private AudioSource collectionSound;
    private Transform player;

    void Start () 
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform; 
		collectionSound = GetComponent<AudioSource> ();
        InvokeRepeating ("CheckIfBehindPlayer", 5f, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cube")
        {
            print("A powerup has spawned inside a cube, moving...");
            transform.position -= Vector3.forward * 100f;
        }
    }

    void CheckIfBehindPlayer()
    {
        if (this.transform.position.z < player.transform.position.z) 
        {
            float x = Random.Range(-300f, 300f);
            float z = Random.Range(600f, 2000f);
            this.transform.position = new Vector3 (player.transform.position.x+x, this.transform.position.y, player.transform.position.z + z);
            //print ("Powerup behind Player, respawning...");
        }
    }

	public void MovePosition()
	{
		float x = Random.Range(-300f, 300f);
		float z = Random.Range(600f, 2000f);
		this.transform.position = new Vector3 (player.transform.position.x+x, this.transform.position.y, player.transform.position.z + z);
	}

	public AudioSource GetAudio()
	{
		return collectionSound;
	}
}

public enum PowerupType {BonusPoints, DoublePoints, Invincibility};