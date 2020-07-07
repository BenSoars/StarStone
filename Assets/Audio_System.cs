using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_System : MonoBehaviour
{
    // Ben Soars
    public AudioSource gunShot; // gun audio source
    public AudioSource playerHurt; // player injured audio source
    public AudioSource other;

    // in lists to make it easier to get access to them
  
    public List<AudioClip> playerHurtSounds = new List<AudioClip>(); // player injured sound effects

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
}
