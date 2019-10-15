using UnityEngine;
using UnityEngine.UI;

namespace Highscore
{
	/// <summary>
	/// A UI representation of a highscore.
	/// </summary>
	public class HighscoreEntry : MonoBehaviour
	{
		/// <summary>
		/// Used in the inspector to keep track of 2 text fields for displaying highscore information.
		/// </summary>	
		[SerializeField] private Text rankText;
		[SerializeField] private Text usernameText;
		[SerializeField] private Text scoreText;

		public void Populate(string rank, string username, string score)
		{
			rankText.text = rank;
			usernameText.text = username;
			scoreText.text = score;
		}
	}
}
