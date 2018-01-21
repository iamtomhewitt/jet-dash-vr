using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour 
{
    public Text distanceScoreText;
    public Text bonusScoreText;
	public Text topSpeed;
	public Text finalScore;

    public void AnimateBonusScore(int score)
    {
        StartCoroutine(Animate(score, bonusScoreText, false));
    }


    public void AnimateDistanceScore(int score)
    {
        StartCoroutine(Animate(score, distanceScoreText, false));
    }


	public void AnimateTopSpeed(int score)
	{
		StartCoroutine(Animate(score, topSpeed, false));
	}


	public void AnimateFinalScore(int score)
	{
		StartCoroutine(Animate(score, finalScore, true));
	}


	IEnumerator Animate(int score, Text text, bool isFinalScore)
    {
        int displayScore = 0;
        int start = displayScore;

        for (float timer = 0; timer < 3f; timer += Time.deltaTime)
        {
            float progress = timer / 3f;
            displayScore = (int)Mathf.Lerp(start, score, progress);
            text.text = displayScore.ToString();
            yield return null;
        }

        displayScore = score;
        text.text = displayScore.ToString();

		// Final score will take the longest to animate, to only reload the scene when animating final score,
		// that way nothing gets cut off
		if (isFinalScore)
			GameObject.FindObjectOfType<LevelManager> ().ReloadScene ();
    }


    public void Show()
    {
        StartCoroutine(Fade());
    }


    IEnumerator Fade()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;

        while (cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime * 1.5f;
            yield return null;
        }

        cg.interactable = false;
        yield return null;
    }
}
