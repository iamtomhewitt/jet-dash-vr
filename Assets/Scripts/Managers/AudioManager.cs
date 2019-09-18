﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Manager
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField] private Sound[] sounds;

		public static AudioManager instance;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this.gameObject);
				return;
			}

			DontDestroyOnLoad(this.gameObject);

			foreach (Sound s in sounds)
			{
				s.source = this.gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
		}

		private void Start()
		{
			Play("Music");
		}

		public void Play(string name)
		{
			Sound s = GetSound(name);

			if (s != null)
			{
				s.source.Play();
			}
		}

		public void Pause(string name)
		{
			Sound s = GetSound(name);

			if (s != null)
			{
				s.source.Pause();
			}
		}
		
		/// <summary>
		/// Adds an AudioSource component to a specific Gameobject.
		/// </summary>
		public void AttachSoundTo(string soundName, GameObject g)
		{
			Sound s = GetSound(soundName);

			s.source = g.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		/// <summary>
		/// Retrieve a sound by name on the AudioManager object.
		/// </summary>
		public Sound GetSound(string name)
		{
			Sound s = Array.Find(sounds, sound => sound.name == name);

			if (s == null)
			{
				print("WARNING! Sound: '" + name + "' was not found.");
				return null;
			}
			return s;
		}

		/// <summary>
		/// A data class to hold information about sounds.
		/// </summary>
		[System.Serializable]
		public class Sound
		{
			public string name;

			public AudioClip clip;

			[Range(0f, 1f)]
			public float volume;

			[Range(0.5f, 3f)]
			public float pitch;

			public bool loop;

			[HideInInspector]
			public AudioSource source;
		}
	}
}
