using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour 
{
	public GameObject cube;
	public int totalCubes;
	public Boundary boundary;

	void Start()
	{
		InitialiseCubes ();
	}

	void InitialiseCubes()
	{
		for (int i = 0; i < totalCubes; i++)
		{
			int scale = Random.Range (5, 11);
			float y = (scale / 2) + 0.5f;
			GameObject c = Instantiate (cube, GenerateCubePosition (y), Quaternion.identity) as GameObject;
			c.transform.localScale = new Vector3 (scale, scale, scale);
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
