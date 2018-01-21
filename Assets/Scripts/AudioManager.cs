using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// Audio that is shared throughout the game
	public AudioSource music;
	public AudioSource UISelection;
	private static AudioManager instance;

	void Awake()
	{
		if (instance) 
		{
			DestroyImmediate (gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}

	void Start()
	{
		music.Play ();
	}
}
