using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour 
{
    public int score = 0;
    private HUD hud;

    void Start()
    {
        hud = GetComponent<HUD>();
    }

    void Update()
    {
        hud.SetScoreText(score);
    }

    public void AddBonusPoints(int points)
    {
        score += points;
    }

    public void DoublePoints()
    {
        score *= 2;
    }
}
