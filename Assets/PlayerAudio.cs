using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour 
{
    public AudioSource death;
    public AudioSource countdown;
    public AudioSource shipHum;

    void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        countdown.Play();
        yield return new WaitForSeconds(.5f);
        countdown.Play();
        yield return new WaitForSeconds(.5f);
        countdown.pitch += .1f;
        //countdown.clip.
        countdown.Play();
    }
}
