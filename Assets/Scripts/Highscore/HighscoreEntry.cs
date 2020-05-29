using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace Highscore
{
	/// <summary>
	/// A UI representation of a highscore.
	/// </summary>
	public class HighscoreEntry : MonoBehaviour
	{
		[SerializeField] private Color first;
		[SerializeField] private Color secondAndThird;
		[SerializeField] private GameObject vrIcon;
		[SerializeField] private Image shipSprite;
		[SerializeField] private Sprite[] shipSprites;
		[SerializeField] private Text rankText;
		[SerializeField] private Text scoreText;
		[SerializeField] private Text usernameText;

		private string[] devColours = new string[] { "#f33214", "#F35514" };

		public void Populate(int rank, string username, string score)
		{
			string formatted = username.StripNonLatinLetters();

			rankText.SetText(rank + ".");
			usernameText.SetText(string.IsNullOrEmpty(formatted) ? "<invalid name>" : formatted);
			scoreText.SetText(score);

			if (Constants.DEVS.Split(',').Contains(username))
			{
				usernameText.SetText(ApplyDevColours(username));
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