﻿using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
	/// <summary>
	/// An achievement in UI form.
	/// </summary>
	public class AchievementEntry : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		[SerializeField] private Text descriptionText;
		[SerializeField] private Text percentageText;
		[SerializeField] private Text awardValueText;

		[SerializeField] private Slider progressSlider;

		[SerializeField] private Image icon;

		[SerializeField] private Sprite unlocked;
		[SerializeField] private Sprite locked;

		public void Populate(string name, string description, float progress, int awardValue)
		{
			nameText.text = name;
			descriptionText.text = description;
			percentageText.text = progress.ToString("F1") + "%";
			progressSlider.value = progress;
			awardValueText.text = awardValue.ToString() + "P";

			// Trim the slight blue on a zero value
			bool show = (progress <= 0f) ? false : true;
			progressSlider.fillRect.gameObject.SetActive(show);

			icon.sprite = progress >= 100f ? unlocked : locked;
		}
	}
}