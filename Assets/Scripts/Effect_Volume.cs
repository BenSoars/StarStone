using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Volume : MonoBehaviour
{
    public AudioSource sound; // used for the music
    // Start is called before the first frame update
    void Start()
    {
        fixVolume();
    }

    public void fixVolume()
    {
        sound.volume = PlayerPrefs.GetFloat("volumeLevel"); // set the music level
        sound.Play(); // play the musi


    }
}
