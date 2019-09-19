using UnityEngine;

public class WebPageButton : MonoBehaviour
{
    public void OpenInBrowser(string url)
	{
		Application.OpenURL(url);
	}
}
