using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Any objects that need to be loaded before the game starts, or need to to be persisted throughout scenes, should be loaded here.
/// <summary>
public class Preload : MonoBehaviour
{
    [SerializeField] private GameObject[] requiredComponents;
    [SerializeField] private string mainMenu = "Main Menu";

    private void Start()
    {
        foreach (GameObject g in requiredComponents)
        {
            Instantiate(g).transform.name += "_PRELOAD";
        }

        SceneManager.LoadScene(mainMenu);
    }
}
