using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private  AudioSource dashSound;
    [SerializeField] private  AudioSource runSound;
    [SerializeField] private  AudioSource landSound;
    [SerializeField] private  AudioSource jumpSound;

    public void dash()
    {
        if(dashSound != null)
        dashSound.Play();
    }
    public void runStart()
    {
        if(runSound != null && !runSound.isPlaying)
        runSound.Play();
    }
    public void runEnd()
    {
        if(runSound != null)
        runSound.Stop();
    }
    public void land()
    {
        if(landSound != null)
        landSound.Play();
    }
    public void jump()
    {
        if(jumpSound != null)
        jumpSound.Play();
    }
}
