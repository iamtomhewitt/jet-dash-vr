using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour 
{
    public Text distanceScoreText;
    public Text bonusScoreText;

    public void AnimateBonusScore(int score)
    {
        StartCoroutine(Animate(score, bonusScoreText));
    }


    public void AnimateDistanceScore(int score)
    {
        StartCoroutine(Animate(score, distanceScoreText));
    }


    IEnumerator Animate(int score, Text text)
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

        GameObject.FindObjectOfType<LevelManager>().ReloadScene();
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
