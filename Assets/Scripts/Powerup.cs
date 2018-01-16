using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour 
{
    public PowerupType powerupType;
}

public enum PowerupType {BonusPoints, DoublePoints, Invincibility};