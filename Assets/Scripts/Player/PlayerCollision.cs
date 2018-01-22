using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject explosion;
    public GameObject gameHUD;
    public GameObject playerModel;

    public Scoreboard scoreboard;

    private HUD hud;
    private PlayerControl playerControl;
    private PlayerScore playerScore;
    private Collider col;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerScore = GetComponent<PlayerScore>();
        hud = GetComponent<HUD>();
        col = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Cube":
                GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                Destroy(e, 3f);

                scoreboard.Show();
                scoreboard.AnimateDistanceScore(playerScore.GetDistanceScore());
                scoreboard.AnimateBonusScore(playerScore.GetBonusScore());
                scoreboard.AnimateTopSpeed(playerScore.GetSpeed());
                scoreboard.AnimateFinalScore(playerScore.GetFinalScore());

                playerControl.StopMoving();
                playerModel.SetActive(false);
                gameHUD.SetActive(false);

                PlayerAudio audio = GetComponent<PlayerAudio>();
                audio.death.Play();
                audio.shipHum.Pause();
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Powerup":
                Powerup powerup = other.GetComponent<Powerup>();
                powerup.GetAudio().Play();

                Color colour = other.GetComponent<Renderer>().material.color;

                switch (powerup.powerupType)
                {
                    case PowerupType.BonusPoints:
                        playerScore.AddBonusPoints(500);
                        hud.ShowPowerupNotification(colour, "+500!");
                        break;

                    case PowerupType.DoublePoints:
                        playerScore.DoublePoints();
                        hud.ShowPowerupNotification(colour, "x2!");
                        break;

                    case PowerupType.Invincibility:
                        StartCoroutine(GodMode(5f));
                        hud.ShowPowerupNotification(colour, "Invincible!");
                        break;
                }

                powerup.MovePosition();
                break;
        }
    }

    IEnumerator GodMode(float duration)
    {
        Vector3 originalScale = new Vector3(15f, 1f, 1f);
        Vector3 targetScale = new Vector3(0f, 1f, 1f);

        float timer = 0f;

        do
        {
            col.enabled = false;
            hud.invincibilityBar.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer <= duration);

        hud.invincibilityBar.transform.localScale = Vector3.zero;
        col.enabled = true;
    }
}
