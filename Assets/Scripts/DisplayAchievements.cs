using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAchievements : MonoBehaviour
{
    // Ben Soars
    public List<GameObject> allAchievements = new List<GameObject>(); // list of all Achievement places
    private AchievementTracker m_Achievement; // the Achievement tracker

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; // reset the time scale
        Invoke("CheckAll", 0.1f); //wait a frame before checking, just in case there are multiple Achievement trackers in the scene
    }

    public void CheckAll()
    {
        m_Achievement = GameObject.FindObjectOfType<AchievementTracker>(); // get the Achievement tracker
        for (int i = 0; i < m_Achievement.unlockImages.Count; i++) // for loop through all the Achievements listed in the tracker
        {
            displayAchievements(i); // display the Achievement and pass through i
        }
    }

    private void displayAchievements(int AchievementID) // display Achievement at passed position
    {
        Image thumbnail = allAchievements[AchievementID].transform.GetChild(2).GetComponent<Image>(); // get the image component from the current Achievement display area
        allAchievements[AchievementID].transform.GetChild(1).GetComponent<Text>().text = m_Achievement.unlockNames[AchievementID]; // set the text to match the current Achievement ID
        thumbnail.sprite = m_Achievement.unlockImages[AchievementID]; // set the Achievement image to match the current ID
        allAchievements[AchievementID].transform.GetChild(3).GetComponent<Text>().text = m_Achievement.unlockDesc[AchievementID]; // update the description of the current Achievement ID
        if (PlayerPrefs.GetInt(m_Achievement.unlockNames[AchievementID]) == 0) // if the passed variable isn't already unlocked
        {
            thumbnail.color = Color.grey; // set the Achievement to be greyed out
        }
    }

    
}
