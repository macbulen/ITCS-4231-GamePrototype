using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip bowPull;
    public AudioClip bowFire;
    public AudioClip fishCaught;
    public bool isPaused = false;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (isPaused == false)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public void setIsPausedTrue()
    {
        isPaused = true;
    }

    public void setIsPausedFalse()
    {
        isPaused = false;
    }

}
