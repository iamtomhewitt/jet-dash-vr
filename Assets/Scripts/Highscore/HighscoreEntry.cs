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

		public void SetUsernameText(string n)
		{
			usernameText.text = n;
		}

		public void SetScoreText(int s)
		{
			scoreText.text = s.ToString();
		}

		public void SetRankText(string r)
		{
			rankText.text = r;
		}
	}
}
