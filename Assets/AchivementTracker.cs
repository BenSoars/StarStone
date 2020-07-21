using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementTracker : MonoBehaviour
{

    // Ben Soars
    public Text achivementName; // the place to display the name
    public Text achivementDesc; // the place to display the description
    public Image achivementImage; // the place to display the image
    public AudioSource achivementSound; // the sound that plays when an unlock happens

    public Animator m_anim; // the animator for the achivement unlocker

    public List<string> unlockNames = new List<string>(); // list of all unlockable names
    public List<Sprite> unlockImages = new List<Sprite>(); // list of all unlockable images
    public List<string> unlockDesc = new List<string>(); // list of all the descriptions

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // carry between scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        resetProgress();
        achivementSound.volume = PlayerPrefs.GetFloat("volumeLevel"); // sets the volume to match the saved volume
    }
       
    public void UnlockAchivement(int checkUnlock)
    {
        if (PlayerPrefs.GetInt(unlockNames[checkUnlock]) == 0) // if the passed variable isn't already unlocked
        {
            // set the displayed objects to update with the unlocked achivement
            achivementDesc.text = unlockDesc[checkUnlock];
            achivementName.text = unlockNames[checkUnlock];
            achivementImage.sprite = unlockImages[checkUnlock];
            m_anim.SetTrigger("Unlock"); // play the unlock animation
            achivementSound.Play(); // play the sound
            PlayerPrefs.SetInt(unlockNames[checkUnlock], 1); // unlock the achivement
            PlayerPrefs.Save(); // save the unlocked achivement
        }
    }

    public void resetProgress()
    {
        for (int i = 0; i < unlockNames.Count; i++)
        {
            PlayerPrefs.SetInt(unlockNames[i], 0); // unlock the achivement
           
        }
        PlayerPrefs.Save(); // save the unlocked achivement
    }


}
