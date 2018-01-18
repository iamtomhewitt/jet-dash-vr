using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTransformPosition : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        transform.position = target.position + offset;
    }
}
