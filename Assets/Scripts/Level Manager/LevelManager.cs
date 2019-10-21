using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;

namespace LevelManagers
{
	/// <summary>
	/// A base class for all Level Managers.
	/// A Level Manager is a local scene manager that controls everything on that scene, such as Game Settings on the main menu, or advert management on the Game scene.
	/// </summary>
	public abstract class LevelManager : MonoBehaviour
	{
		/// <summary>
		/// Loads a specified level.
		/// </summary>
		public void LoadLevel(string levelName)
		{
			SceneManager.LoadScene(levelName);
		}

		/// <summary>
		/// Called when a generic button is pressed. This method should be called before doing any specific button methods.
		/// </summary>
		public void OnButtonPressed()
		{
			AudioManager.instance.Play(SoundNames.BUTTON_SELECT);
		}
	}
}