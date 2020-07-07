using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    // Ben Soars
    public Slider volumeSlider;

    void Start()
    {
        FirstTime();
        volumeSlider.value = PlayerPrefs.GetFloat("volumeLevel");
    }

    void FirstTime() {
        if (PlayerPrefs.GetFloat("volumeLevel") == 0)
        {
            PlayerPrefs.SetFloat("volumeLevel", 0.8f);
            PlayerPrefs.Save();
        }
       
    }

    public void alterSoundVolume()
    {
        PlayerPrefs.SetFloat("volumeLevel", volumeSlider.value);
        PlayerPrefs.Save();
    }
}
