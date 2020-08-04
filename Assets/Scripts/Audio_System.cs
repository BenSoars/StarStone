using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_System : Music_Volume
{
    // Ben Soars
    public AudioSource gunShot; // gun audio source
    public AudioSource playerHurt; // player injured audio source
    public AudioSource other;
    public AudioSource notification; // used for player notifications, such as round start or important annoucenemnts
    public AudioSource enemyHurt; // enemy injured sound effect

    

    // in lists to make it easier to get access to them
  
    public List<AudioClip> playerHurtSounds = new List<AudioClip>(); // player injured sound effects

    void Start()
    {   // set the volume to match the volue passed from the menu
        gunShot.volume = PlayerPrefs.GetFloat("volumeLevel");
        playerHurt.volume = PlayerPrefs.GetFloat("volumeLevel");
        other.volume = PlayerPrefs.GetFloat("volumeLevel");
        notification.volume = PlayerPrefs.GetFloat("volumeLevel");
        enemyHurt.volume = PlayerPrefs.GetFloat("volumeLevel") / 1.2f;
       
    }

    public void playGun(AudioClip gunSound) // 
    {
        gunShot.clip = gunSound; // get the passed through audio clip
        gunShot.Play(); // play the sound
    }

    public void playPlayerHurt()
    {
        playerHurt.clip = playerHurtSounds[Random.Range(0, playerHurtSounds.Count)]; // get a random hurt sound
        playerHurt.Play(); // play the sound
    }

    public void playOther(AudioClip soundClip) // is separate from gun shot to avoid gun shot sounds from ending too early
    {
        other.clip = soundClip; // get the passed through audio clip
        other.Play(); // play the sound
    }

    public void playImportant(AudioClip soundClip) // is separate for the same reason as play other is. Has higher priority
    {
        notification.clip = soundClip; // get the passed through audio clip
        notification.Play(); // play the sound
    }

    public void playEnemyHurt() // Enemy hurt sound play, is separate as it would cut off otherwise
    {
        enemyHurt.pitch = Random.Range(0.7f, 1.31f); // set the pitch to be random
        enemyHurt.Play(); // play the sound
    }
}
