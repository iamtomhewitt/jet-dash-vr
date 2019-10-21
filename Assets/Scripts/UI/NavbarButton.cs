using UnityEngine;
using LevelManagers;

namespace UI
{
	public class NavbarButton : MonoBehaviour
	{
		/// <summary>
		/// Called from a Unity button. Used for the navbar at the bottom of a screen, saves
		/// us having to hook up a level manager on each scene. This way the prefab will always be
		/// able to load levels without having to hook anything up in the inspector.
		/// </summary>
		public void SwitchLevel(string scene)
		{
			LevelManager manager = FindObjectOfType<LevelManager>();
			manager.OnButtonPressed();
			manager.LoadLevel(scene);
		}
	}
}
