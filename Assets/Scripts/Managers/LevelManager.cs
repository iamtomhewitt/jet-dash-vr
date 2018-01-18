using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
    public void ReloadScene()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        print("todo, advert after so many restarts");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
