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

    private bool m_isActive;
    public AchivementTracker[] m_extraAchivements; // used to make sure there aren't multiple achivement handlers in the scene

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // carry between scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("becomeActive", 0.2f);
        achivementSound.volume = PlayerPrefs.GetFloat("volumeLevel"); // sets the volume to match the saved volume
    }

    void FixedUpdate()
    {
        if (m_isActive)
        {
            m_extraAchivements = FindObjectsOfType<AchivementTracker>();
            if (m_extraAchivements.Length > 1)
            {
                for (int i = 0; i < m_extraAchivements.Length; i++)
                {
                    if (m_extraAchivements[i] == this)
                    {
                        Destroy(m_extraAchivements[i].gameObject);
                    }
                }
            }
        }
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

    private void becomeActive()
    {
        m_isActive = true;
    }

}
