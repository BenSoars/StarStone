using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementTracker : MonoBehaviour
{

    // Ben Soars
    public Text AchievementName; // the place to display the name
    public Text AchievementDesc; // the place to display the description
    public Image AchievementImage; // the place to display the image
    public AudioSource AchievementSound; // the sound that plays when an unlock happens

    public Animator m_anim; // the animator for the Achievement unlocker

    public List<string> unlockNames = new List<string>(); // list of all unlockable names
    public List<Sprite> unlockImages = new List<Sprite>(); // list of all unlockable images
    public List<string> unlockDesc = new List<string>(); // list of all the descriptions

    private bool m_isActive;
    public AchievementTracker[] m_extraAchievements; // used to make sure there aren't multiple Achievement handlers in the scene

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // carry between scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("becomeActive", 0.2f);
        AchievementSound.volume = PlayerPrefs.GetFloat("volumeLevel"); // sets the volume to match the saved volume
        
    }

    void FixedUpdate()
    {
        if (m_isActive)
        {
            m_extraAchievements = FindObjectsOfType<AchievementTracker>();
            if (m_extraAchievements.Length > 1)
            {
                for (int i = 0; i < m_extraAchievements.Length; i++)
                {
                    if (m_extraAchievements[i] == this)
                    {
                        Destroy(m_extraAchievements[i].gameObject);
                    }
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Tutorial_Scene")
        {
            Destroy(gameObject);
        }
    }
       
    public void UnlockAchievement(int checkUnlock)
    {
        if (PlayerPrefs.GetInt(unlockNames[checkUnlock]) == 0) // if the passed variable isn't already unlocked
        {
            // set the displayed objects to update with the unlocked Achievement
            AchievementDesc.text = unlockDesc[checkUnlock];
            AchievementName.text = unlockNames[checkUnlock];
            AchievementImage.sprite = unlockImages[checkUnlock];
            m_anim.SetTrigger("Unlock"); // play the unlock animation
            AchievementSound.Play(); // play the sound
            PlayerPrefs.SetInt(unlockNames[checkUnlock], 1); // unlock the Achievement
            PlayerPrefs.Save(); // save the unlocked Achievement
        }
    }

    public void resetProgress()
    {
        for (int i = 0; i < unlockNames.Count; i++)
        {
            PlayerPrefs.SetInt(unlockNames[i], 0); // unlock the Achievement
           
        }
        PlayerPrefs.SetInt("TotalKills", 0);
        PlayerPrefs.Save(); // save the unlocked Achievement
    }

    private void becomeActive()
    {
        m_isActive = true;
    }

}
