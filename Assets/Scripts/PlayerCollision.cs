using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerControl playerControl;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Cube":
                print("Hit a cube!");
                playerControl.StopMoving();
                break;

            case "Powerup":
                print("Hit a powerup!");
                break;
        }
    }
}
