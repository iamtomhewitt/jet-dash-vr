using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class Powerup : MonoBehaviour
    {
        [SerializeField] private PowerupType powerupType;
		[SerializeField] private Color32 colour;

        private Transform player;

		private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; 
            InvokeRepeating("CheckIfBehindPlayer", 5f, 5f);
        }

		private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Obstacle")
            {
                print("A powerup has spawned inside an obstacle, moving...");
                transform.position -= Vector3.forward * 100f;
            }
        }

		private void CheckIfBehindPlayer()
        {
            if (this.transform.position.z < player.transform.position.z)
            {
                float x = Random.Range(-300f, 300f);
                float z = Random.Range(600f, 2000f);
                this.transform.position = new Vector3(player.transform.position.x + x, this.transform.position.y, player.transform.position.z + z);
                //print ("Powerup behind Player, respawning...");
            }
        }

        public void MovePosition()
        {
            float x = Random.Range(-300f, 300f);
            float z = Random.Range(600f, 2000f);
            this.transform.position = new Vector3(player.transform.position.x + x, this.transform.position.y, player.transform.position.z + z);
        }

		public PowerupType GetPowerupType()
		{
			return powerupType;
		}

		public Color32 GetColour()
		{
			return colour;
		}
    }

    public enum PowerupType
    {
        BonusPoints,
        DoublePoints,
        Invincibility
    };
}