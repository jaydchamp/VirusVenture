using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip MPlatforming; // Assign your first audio clip here
    public AudioClip MPChallenge; // Assign your second audio clip here

    private AudioSource audioSource;
    private bool isPlayingPlatformingMusic = true;

    public static MusicManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MPlatforming;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SwitchMusic()
    {
        if (isPlayingPlatformingMusic)
        {
            //Debug.Log("Switched TO Challenege Music");
            audioSource.Stop();
            audioSource.clip = MPChallenge;
            audioSource.Play();
            isPlayingPlatformingMusic = false;
        }
        else
        {
            //Debug.Log("Switched TO PLATFORMING MUSIC");
            audioSource.Stop();
            audioSource.clip = MPlatforming;
            audioSource.Play();
            isPlayingPlatformingMusic = true;
        }
    }

    public void SwitchToPlatformingMusic()
    {
        if (!isPlayingPlatformingMusic)
        {
            //Debug.Log("Switched TO Platformingx  Music");
            audioSource.Stop();
            audioSource.clip = MPlatforming;
            audioSource.Play();
            isPlayingPlatformingMusic = true;
        }
    }

    public void SwitchToChallengeMusic()
    {
        if (isPlayingPlatformingMusic)
        {
            //Debug.Log("Switched TO Challenege Music");
            audioSource.Stop();
            audioSource.clip = MPChallenge;
            audioSource.Play();
            isPlayingPlatformingMusic = false;
        }
    }

    //returns true if playing platformer music, false if challenege music
    public bool GetMusicStatus()
    {
        return isPlayingPlatformingMusic;
    }
}
