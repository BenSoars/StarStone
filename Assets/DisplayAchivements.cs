using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAchivements : MonoBehaviour
{
    // Ben Soars
    public List<GameObject> allAchivements = new List<GameObject>();
    private AchivementTracker m_achivement;

    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 1;
        Invoke("CheckAll", 0.1f);
    }

    public void CheckAll()
    {
        m_achivement = GameObject.FindObjectOfType<AchivementTracker>();
        for (int i = 0; i < m_achivement.unlockImages.Count; i++)
        {
            displayAchivements(i);
        }
    }

    private void displayAchivements(int achivementID)
    {
        Image thumbnail = allAchivements[achivementID].transform.GetChild(2).GetComponent<Image>();
        allAchivements[achivementID].transform.GetChild(1).GetComponent<Text>().text = m_achivement.unlockNames[achivementID];
        thumbnail.sprite = m_achivement.unlockImages[achivementID];
        allAchivements[achivementID].transform.GetChild(3).GetComponent<Text>().text = m_achivement.unlockDesc[achivementID];
        if (PlayerPrefs.GetInt(m_achivement.unlockNames[achivementID]) == 0) // if the passed variable isn't already unlocked
        {
            thumbnail.color = Color.grey;
        }
    }

    
}
