using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerControl playerControl;
    PlayerScore playerScore;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerScore = GetComponent<PlayerScore>();
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Cube":
                print("Hit a cube!");
                playerControl.StopMoving();
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Powerup":
                Powerup powerup = other.GetComponent<Powerup>();
                print("Hit a " + powerup.powerupType + " powerup!");

                switch (powerup.powerupType)
                {
                    case PowerupType.BonusPoints:
                        playerScore.AddBonusPoints(500);
                        break;

                    case PowerupType.DoublePoints:
                        playerScore.DoublePoints();
                        break;

                    case PowerupType.Invincibility:
                        StartCoroutine(GodMode());
                        break;
                }

                Destroy(other.gameObject);
                break;
        }
    }

    IEnumerator GodMode()
    {
        print("Todo, UI for display god mode");
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2f);
        GetComponent<Collider>().enabled = true;
    }
}
