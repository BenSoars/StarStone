using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAchivement : MonoBehaviour // used to unlock an achivement upon entering a secene or by enabling a certain gameobject
{
    public int AchivementID; // the ID of the achivement that
    private AchivementTracker m_achivement;
    // Start is called before the first frame update
    void Start()
    {
        m_achivement = GameObject.FindObjectOfType<AchivementTracker>(); //get the achivement tracker system
        m_achivement.UnlockAchivement(AchivementID); //unlock the passed achivement from the start
    }
}
