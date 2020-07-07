using Manager;
using Player;
using UnityEngine;
using Utility;

namespace SpawnableObject
{
	/// <summary>
	/// Applies a powerup when flown through.
	/// </summary>
	public abstract class Powerup : SpawnableObject
	{
		[SerializeField] private PowerupType powerupType;
		[SerializeField] private Color32 colour;

		private AudioManager audioManager;
		private PlayerHud playerHud;

		public override void Start()
		{
			base.Start();
			audioManager = AudioManager.instance;
			playerHud = FindObjectOfType<PlayerHud>();
		}

		/// <summary>
		/// For example, hitting the double points powerup doubles the players points.
		/// </summary>
		public abstract void ApplyPowerupEffect();

		private void OnTriggerEnter(Collider other)
		{
			switch (other.gameObject.tag)
			{
				case Tags.OBSTACLE:
					Vector3 newPosition = transform.position -= Vector3.forward * 100f;
					Debug.Log(string.Format("A powerup has spawned inside an obstacle, moving from {0} to {1}", transform.position, newPosition));
					transform.position = newPosition;
					break;

				default:
					// Nothing to do!
					break;
			}
		}

		public PowerupType GetPowerupType()
		{
			return powerupType;
		}

		public Color32 GetColour()
		{
			return colour;
		}

		public AudioManager GetAudioManager()
		{
			return audioManager;
		}

		public PlayerHud GetPlayerHud()
		{
			return playerHud;
		}

		public override void AfterRelocation()
		{
			// Nothing to do!
		}
	}

	public enum PowerupType
	{
		BonusPoints,
		DoublePoints,
		Invincibility,
		Jump,
		Hyperdrive,
		SpeedUp,
		SlowDown
	};
}