using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Volume : MonoBehaviour
{
    public AudioSource music; // used for the music
    // Start is called before the first frame update
    void Start()
    {
        
            music.volume = PlayerPrefs.GetFloat("musicLevel"); // set the music level
            music.Play(); // play the music
        
    }

  
}
