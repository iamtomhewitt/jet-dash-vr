using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
	public class LeaderboardTab : MonoBehaviour
	{
		[SerializeField] private GameObject[] leaderboards;
		[SerializeField] private Color activeButtonColour;
		[SerializeField] private Color activeTextColour;
		[SerializeField] private Text scoreTitle;

		private GameObject[] tabButtons;

		private void Start()
		{
			tabButtons = GameObject.FindGameObjectsWithTag(Tags.LEADERBOARD_TAB);
			ShowLeaderboard(leaderboards[0]);
		}

		public void ShowLeaderboard(GameObject leaderboard)
		{
			HideAllLeaderboards();
			DeactivateAllButtons();
			ActivateButton(this.GetComponent<Button>());

			scoreTitle.text = this.GetComponentInChildren<Text>().text;
			leaderboard.SetActive(true);
		}

		private void HideAllLeaderboards()
		{
			foreach (GameObject leaderboard in leaderboards)
			{
				leaderboard.SetActive(false);
			}
		}

		private void ActivateButton(Button button)
		{
			button.GetComponent<Image>().color = activeButtonColour;
			button.GetComponentInChildren<Text>().color = activeTextColour;
		}

		private void DeactivateAllButtons()
		{
			foreach (GameObject button in tabButtons)
			{
				DeactivateButton(button.GetComponent<Button>());
			}
		}

		private void DeactivateButton(Button button)
		{
			// All we do is invert the colours
			button.GetComponent<Image>().color = activeTextColour;
			button.GetComponentInChildren<Text>().color = activeButtonColour;
		}
	}
}