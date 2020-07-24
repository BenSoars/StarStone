using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAchievement : MonoBehaviour // used to unlock an Achievement upon entering a secene or by enabling a certain gameobject
{
    public int AchievementID; // the ID of the Achievement that
    private AchievementTracker m_Achievement;
    // Start is called before the first frame update
    void Start()
    {
        m_Achievement = GameObject.FindObjectOfType<AchievementTracker>(); //get the Achievement tracker system
        m_Achievement.UnlockAchievement(AchievementID); //unlock the passed Achievement from the start
    }
}
