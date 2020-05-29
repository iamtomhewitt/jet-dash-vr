using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Achievements
{
	/// <summary>
	/// An achievement in UI form.
	/// </summary>
	public class AchievementEntry : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[SerializeField] private Slider progressSlider;
		[SerializeField] private Sprite locked;
		[SerializeField] private Sprite unlocked;
		[SerializeField] private Text awardValueText;
		[SerializeField] private Text descriptionText;
		[SerializeField] private Text nameText;
		[SerializeField] private Text percentageText;

		public void Populate(string name, string description, float progress, int awardValue)
		{
			nameText.SetText(name);
			descriptionText.SetText(description);
			percentageText.SetText(progress.ToString("F1") + "%");
			progressSlider.value = progress;
			awardValueText.SetText(awardValue.ToString() + "P");

			// Trim the slight blue on a zero value
			bool show = (progress <= 0f) ? false : true;
			progressSlider.fillRect.gameObject.SetActive(show);

			icon.sprite = progress >= 100f ? unlocked : locked;
		}
	}
}