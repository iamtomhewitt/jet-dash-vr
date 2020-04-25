using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class LeaderboardTab : MonoBehaviour
    {
        [SerializeField] private Color activeButtonColour;
        [SerializeField] private Color activeTextColour;

        private GameObject[] leaderboards;
        private GameObject[] tabButtons;

        private void Start()
        {
            leaderboards = GameObject.FindGameObjectsWithTag(Tags.LEADERBOARD);
            tabButtons = GameObject.FindGameObjectsWithTag(Tags.LEADERBOARD_TAB);
        }

        public void ShowLeaderboard(GameObject leaderboard)
        {
            HideAllLeaderboards();
            DeactivateAllButtons();
            ActivateButton(this.GetComponent<Button>());
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