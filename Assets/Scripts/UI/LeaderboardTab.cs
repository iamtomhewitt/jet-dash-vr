using UnityEngine.UI;
using UnityEngine;
using Utility;

namespace UI
{
	public class LeaderboardTab : MonoBehaviour
	{
		[SerializeField] private GameObject[] leaderboards;
		[SerializeField] private Color activeButtonColour;
		[SerializeField] private Color inactiveButtonColour;
		[SerializeField] private Color activeUnderlineColour;
		[SerializeField] private Color inactiveUnderlineColour;
		[SerializeField] private Color activeTextColour;
		[SerializeField] private Color inactiveTextColour;
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
			ActivateButton(GetComponent<Button>());

			scoreTitle.SetText(GetComponentInChildren<Text>().text);
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
			button.transform.GetChild(1).GetComponentInChildren<Image>().color = activeUnderlineColour;
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
			button.GetComponent<Image>().color = inactiveButtonColour;
			button.GetComponentInChildren<Text>().color = inactiveTextColour;
			button.transform.GetChild(1).GetComponentInChildren<Image>().color = inactiveUnderlineColour;
		}
	}
}