using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    // Ben Soars
    public Slider volumeSlider;
    public AudioSource testSound; // the test sound that plays when altering volume
    public Text displayTotalKills;
 
    
    void Start()
    {
        FirstTime();
        volumeSlider.value = PlayerPrefs.GetFloat("volumeLevel");
        displayTotalKills.text = PlayerPrefs.GetInt("TotalKills").ToString();
    }

    void FirstTime() {
        if (PlayerPrefs.GetFloat("volumeLevel") == 0) // if this is the player's first time playing, set volume to a decent level
        {
            PlayerPrefs.SetFloat("volumeLevel", 0.8f);
            PlayerPrefs.Save(); 
        }
       
    }

    public void alterSoundVolume()
    {
        PlayerPrefs.SetFloat("volumeLevel", volumeSlider.value); // set volume to reflect the slider
        testSound.volume = volumeSlider.value; // set test sound to use that volume
        testSound.Play(); // play the test sound
        PlayerPrefs.Save(); // save the volume level
    }
}
