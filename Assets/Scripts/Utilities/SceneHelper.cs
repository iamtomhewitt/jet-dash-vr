using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A persistent GameObject for helping with scene management.
/// </summary>
namespace Utilities
{
	public class SceneHelper : MonoBehaviour
	{
		public void LoadScene(string s)
		{
			SceneManager.LoadScene(s);
		}
	}
}
