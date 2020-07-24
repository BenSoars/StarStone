using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAchivements : MonoBehaviour
{
    // Ben Soars
    public List<GameObject> allAchivements = new List<GameObject>(); // list of all achivement places
    private AchivementTracker m_achivement; // the achivement tracker

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; // reset the time scale
        Invoke("CheckAll", 0.1f); //wait a frame before checking, just in case there are multiple achivement trackers in the scene
    }

    public void CheckAll()
    {
        m_achivement = GameObject.FindObjectOfType<AchivementTracker>(); // get the achivement tracker
        for (int i = 0; i < m_achivement.unlockImages.Count; i++) // for loop through all the achivements listed in the tracker
        {
            displayAchivements(i); // display the achivement and pass through i
        }
    }

    private void displayAchivements(int achivementID) // display achivement at passed position
    {
        Image thumbnail = allAchivements[achivementID].transform.GetChild(2).GetComponent<Image>(); // get the image component from the current achivement display area
        allAchivements[achivementID].transform.GetChild(1).GetComponent<Text>().text = m_achivement.unlockNames[achivementID]; // set the text to match the current achivement ID
        thumbnail.sprite = m_achivement.unlockImages[achivementID]; // set the achivement image to match the current ID
        allAchivements[achivementID].transform.GetChild(3).GetComponent<Text>().text = m_achivement.unlockDesc[achivementID]; // update the description of the current achivement ID
        if (PlayerPrefs.GetInt(m_achivement.unlockNames[achivementID]) == 0) // if the passed variable isn't already unlocked
        {
            thumbnail.color = Color.grey; // set the achivement to be greyed out
        }
    }

    
}
