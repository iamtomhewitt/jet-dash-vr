using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

		[SerializeField] private Color first;
		[SerializeField] private Color secondAndThird;

		[SerializeField] private Image shipSprite;
		[SerializeField] private GameObject vrIcon;
		[SerializeField] private Sprite[] shipSprites;

		private string[] devColours = new string[] { "red", "orange", "yellow", "lime", "blue", "magenta" };

		public void Populate(string rank, string username, string score)
		{
			rankText.text = rank;
			usernameText.text = username;
			scoreText.text = score;

			if (username.Equals("Tom (The Developer)"))
			{
				usernameText.text = ApplyDevColours(username);
			}
		}

		private string ApplyDevColours(string username)
		{
			string newUsername = "";
			int index = 0;
			foreach (char c in username)
			{
				newUsername += ColourCharacter(c, devColours[index]);
				if (c != ' ') index = ++index > devColours.Length - 1 ? 0 : index;
			}
			return newUsername;
		}

		private string ColourCharacter(char s, string colour)
		{
			return "<color=" + colour + ">" + s + "</color>";
		}

		public void SetIcons(string additonalInfo)
		{
			string[] parts = additonalInfo.Split('|');
			string shipName = parts[0];
			bool vrMode = bool.Parse(parts[1]);

			shipSprite.sprite = shipSprites.Where(s => s.name.Equals(shipName)).First();
			vrIcon.SetActive(vrMode);
		}

		public void SetTextColourBasedOnRank(int rank)
		{
			if (rank <= 1)
			{
				SetColours(first);
			}
			else if (rank >= 4)
			{
				SetColours(Color.white);
			}
			else
			{
				SetColours(secondAndThird);
			}
		}

		private void SetColours(Color colour)
		{
			rankText.color = colour;
			usernameText.color = colour;
			scoreText.color = colour;
		}
	}
}
