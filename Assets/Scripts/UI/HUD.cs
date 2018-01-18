using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
	public Text speedText;
	public Text distanceText;
    public Text scoreText;

    public void SetDistanceText(float distance)
    {
        distanceText.text = distance.ToString("F0");
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
