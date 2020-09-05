using System;
using UnityEngine;

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
			Play(SoundNames.MUSIC);
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
		/// Retrieve a sound by name on the AudioManager object.
		/// </summary>
		public Sound GetSound(string name)
		{
			Sound s = Array.Find(sounds, sound => sound.name == name);

			if (s == null)
			{
				Debug.Log("WARNING! Sound: '" + name + "' was not found.");
				return null;
			}
			return s;
		}

		public Sound[] GetSounds()
		{
			return sounds;
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

	public class SoundNames
	{
		public const string ACHIEVEMENT_UNLOCKED = "Achievement Unlocked";
		public const string BONUS_POINTS = "Powerup Bonus Points";
		public const string BUTTON_SELECT = "UI Select";
		public const string DOUBLE_POINTS = "Powerup Double Points";
		public const string HYPERDRIVE = "Powerup Hyperdrive";
		public const string INVINCIBILITY_POINTS = "Powerup Invincibility";
		public const string JUMP = "Powerup Jump";
		public const string MUSIC = "Music";
		public const string NEW_HIGHSCORE = "New Highscore";
		public const string PLAYER_DEATH = "Player Death";
		public const string SCORE = "Score";
		public const string SHIP_ENGINE = "Ship Hum";
		public const string SHOP_REJECT_PURCHASE = "Shop Reject Purchase";
		public const string SHOP_SELECT_SHIP = "Shop Select Ship";
		public const string SHOP_SPEND_CASH = "Shop Spend Cash";
		public const string SLOW_DOWN = "Powerup Slow Down";
		public const string SPEED_STREAK = "Speed Streak";
		public const string SPEED_UP = "Powerup Speed Up";
	}
}