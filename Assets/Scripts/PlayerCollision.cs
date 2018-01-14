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
                playerControl.speed = 0f;     
                playerControl.CancelInvoke("IncreaseSpeed");
                break;
        }
    }
}
