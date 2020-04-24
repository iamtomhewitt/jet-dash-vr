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

		public void Populate(string rank, string username, string score)
		{
			rankText.text = rank;
			usernameText.text = username;
			scoreText.text = score;
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
