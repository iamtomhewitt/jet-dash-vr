using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
public class Rotate : MonoBehaviour 
{
    public Vector3 speed;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
}