using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebPageButton : MonoBehaviour
{
    public void OpenInBrowser(string url)
	{
		Application.OpenURL(url);
	}
}
