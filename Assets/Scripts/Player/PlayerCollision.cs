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

    private PlayerControl playerControl;
    private PlayerScore playerScore;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerScore = GetComponent<PlayerScore>();
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Cube":
                GameObject e = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                Destroy(e, 3f);

                playerControl.StopMoving();
                gameHUD.SetActive(false);
                playerModel.SetActive(false);

                scoreboard.Show();
                scoreboard.AnimateDistanceScore(playerScore.GetDistanceScore());
                scoreboard.AnimateBonusScore(playerScore.GetBonusScore());
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Powerup":
                Powerup powerup = other.GetComponent<Powerup>();
                print("Hit a " + powerup.powerupType + " powerup!");

                switch (powerup.powerupType)
                {
                    case PowerupType.BonusPoints:
                        playerScore.AddBonusPoints(500);
                        break;

                    case PowerupType.DoublePoints:
                        playerScore.DoublePoints();
                        break;

                    case PowerupType.Invincibility:
                        StartCoroutine(GodMode());
                        break;
                }

                Destroy(other.gameObject);
                break;
        }
    }

    IEnumerator GodMode()
    {
        print("Todo, UI for display god mode");
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2f);
        GetComponent<Collider>().enabled = true;
    }
}
