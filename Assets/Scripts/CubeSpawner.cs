using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour 
{
	public GameObject cube;
	public Boundary boundary;
    public CubeSize cubeSize;
    public int totalCubes;

	void Start()
	{
		InitialiseCubes ();
	}

	void InitialiseCubes()
	{
		for (int i = 0; i < totalCubes; i++)
		{
            // Work out a scale and how high the cube has to be so it sits nicely on top of the floor
            int scale = Random.Range (cubeSize.minSize, cubeSize.maxSize);
			float y = (scale / 2) + 0.5f;

			GameObject c = Instantiate (cube, GenerateCubePosition (y), Quaternion.identity) as GameObject;
			c.transform.localScale = new Vector3 (scale, scale, scale);
            c.transform.parent = GameObject.Find("Cubes").transform;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cube") 
		{
			other.transform.position = GenerateCubePosition(other.transform.position.y);
		}
	}

	Vector3 GenerateCubePosition(float y)
	{
		float x = Random.Range (boundary.xMin, boundary.xMax);
		float z = Random.Range (boundary.zMin, boundary.zMax);

		return new Vector3(transform.position.x + x, y, transform.position.z + z);
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
