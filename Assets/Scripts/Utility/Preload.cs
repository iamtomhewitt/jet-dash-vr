using UnityEngine.SceneManagement;
using UnityEngine;

namespace Utility
{
	/// <summary>
	/// Any objects that need to be loaded before the game starts, or need to to be persisted throughout scenes, should be loaded here.
	/// <summary>
	public class Preload : MonoBehaviour
	{
		[SerializeField] private GameObject[] requiredComponents;

		private void Start()
		{
			foreach (GameObject g in requiredComponents)
			{
				Instantiate(g).transform.name += "_PRELOAD";
			}

			SceneManager.LoadScene(Menus.MAIN_MENU);
		}
	}
}