using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class Obstacle : MonoBehaviour
    {
        private Transform player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;	

            InvokeRepeating("CheckIfBehindPlayer", 5f, 5f);
        }

        void CheckIfBehindPlayer()
        {
            if (this.transform.position.z < player.transform.position.z - 30f)
            {
                float x = Random.Range(-300f, 300f);
                float z = Random.Range(600f, 2000f);
                this.transform.position = new Vector3(player.transform.position.x + x, this.transform.position.y, player.transform.position.z + z);
                //print ("Cube behind Player, respawning...");
                Grow(.5f, (int)transform.localScale.x);
            }
        }

        public void Grow(float duration, int scale)
        {
            StartCoroutine(GrowIE(duration, scale));
        }

        IEnumerator GrowIE(float duration, int scale)
        {
            Vector3 originalScale = Vector3.zero;
            Vector3 targetScale = new Vector3(scale, scale, scale);

            float timer = 0f;

            do
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            while (timer <= duration);

            transform.localScale = targetScale;
        }
    }
}
