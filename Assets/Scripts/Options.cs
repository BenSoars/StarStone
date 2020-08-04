using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    // Ben Soars
    public Slider volumeSlider;
    public Slider musicSlider;
    public AudioSource testSound; // the test sound that plays when altering volume
    public AudioSource testMusic; // the music that plays on the main menu
    public Text displayTotalKills;
 
    
    void Start()
    {
        FirstTime();
        volumeSlider.value = PlayerPrefs.GetFloat("volumeLevel");
        musicSlider.value = PlayerPrefs.GetFloat("musicLevel");
        displayTotalKills.text = "Total Kills: " + PlayerPrefs.GetInt("TotalKills").ToString();
    }

    void FirstTime() {
        // if this is the player's first time playing, set volume to a decent level
        if (PlayerPrefs.GetFloat("volumeLevel") == 0) 
        {
            PlayerPrefs.SetFloat("volumeLevel", 0.8f);
        }

        if (PlayerPrefs.GetFloat("musicLevel") == 0)
        {
            PlayerPrefs.SetFloat("musicLevel", 0.8f);
            
        }
        PlayerPrefs.Save(); // save the file
    }

    public void alterSoundVolume()
    {
        PlayerPrefs.SetFloat("volumeLevel", volumeSlider.value); // set volume to reflect the slider
        testSound.volume = volumeSlider.value; // set test sound to use that volume
        testSound.Play(); // play the test sound
        PlayerPrefs.Save(); // save the volume level
    }

    public void alterMusicVolume()
    {
        PlayerPrefs.SetFloat("musicLevel", musicSlider.value); // set volume to reflect the slider
   
        PlayerPrefs.Save(); // save the volume level
    }
}
