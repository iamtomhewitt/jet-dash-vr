using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// A custom tab.
	/// </summary>
	public class Tab : MonoBehaviour
	{
		public GameObject content;
		public Color activeColour;
		public Color inactiveColour;

		public bool activeOnStart = false;

		private void Start()
		{
			if (activeOnStart)
			{
				Activate(true);
			}
		}

		/// <summary>
		/// Called from a button.
		/// </summary>
		public void Activate(bool on)
		{
			DeactivateOtherTabs();
			GetComponent<Image>().color = activeColour;
			content.SetActive(true);
		}

		private void DeactivateOtherTabs()
		{
			Tab[] tabs = FindObjectsOfType<Tab>();

			foreach (Tab tab in tabs)
			{
				tab.content.SetActive(false);
				tab.GetComponent<Image>().color = inactiveColour;
			}
		}
	}
}