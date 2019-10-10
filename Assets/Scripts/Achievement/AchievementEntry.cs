using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementEntry : MonoBehaviour
{
	[SerializeField] private Text nameText;
	[SerializeField] private Text descriptionText;
	[SerializeField] private Text percentageText;
	[SerializeField] private Text awardValueText;

	[SerializeField] private Slider progressSlider;

	public void Populate(string name, string description, float progress, int awardValue)
	{
		nameText.text = name;
		descriptionText.text = description;
		percentageText.text = progress.ToString() + "%";
		progressSlider.value = progress;
		awardValueText.text = awardValue.ToString();

		bool show = (progress <= 0f) ? false : true;
		progressSlider.fillRect.gameObject.SetActive(show);
	}
}
