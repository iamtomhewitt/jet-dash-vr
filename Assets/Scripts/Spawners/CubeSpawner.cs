﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour 
{
	public GameObject cube;
    public int totalCubes;

	public Boundary boundary;
    public Boundary initialBoundary;
    public CubeSize cubeSize;

	void Start()
	{
		InitialiseCubes ();
        InvokeRepeating("ReduceSpawnBoundary", 30f, 30f);
	}

	void InitialiseCubes()
	{
		for (int i = 0; i < totalCubes; i++)
		{
            // Work out a scale and how high the cube has to be so it sits nicely on top of the floor
            int scale = Random.Range (cubeSize.minSize, cubeSize.maxSize);
			float y = (scale / 2) + 0.5f;

            GameObject c = Instantiate (cube, GenerateCubePosition (initialBoundary, y), Quaternion.identity) as GameObject;
			c.GetComponent<Cube> ().Grow (1f, scale);
			c.transform.localScale = new Vector3 (scale, scale, scale);
            c.transform.parent = GameObject.Find("Cubes").transform;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cube") 
		{
			other.transform.position = GenerateCubePosition(boundary, other.transform.position.y);
			other.GetComponent<Cube> ().Grow (.5f, (int)other.transform.localScale.x);
		}
	}

	Vector3 GenerateCubePosition(Boundary b, float y)
	{
		float x = Random.Range (b.xMin, b.xMax);
		float z = Random.Range (b.zMin, b.zMax);

		return new Vector3(transform.position.x + x, y, transform.position.z + z);
	}

    void ReduceSpawnBoundary()
    {
        if (boundary.zMin >= 150)
            boundary.zMin -= 50;

        if (boundary.zMax >= 400)
            boundary.zMax -= 50;
    }
}

[System.Serializable]
public class Boundary
{
	public float xMin, xMax;
	public float zMin, zMax;
}

[System.Serializable]
public class CubeSize
{
    public int minSize, maxSize;
}