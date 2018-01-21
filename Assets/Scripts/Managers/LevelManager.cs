using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	private AdvertManager advertManager;

	void Start()
	{
		advertManager = GameObject.FindObjectOfType<AdvertManager> ();
	}

    public void ReloadScene()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(4f);

		advertManager.advertCounter++;

		if (advertManager.advertCounter >= 3) 
		{
			advertManager.ShowAdvert ();
			advertManager.advertCounter = 0;
		}


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
}
