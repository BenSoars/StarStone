using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAchivement : MonoBehaviour
{
    public int AchivementID;
    private AchivementTracker m_achivement;
    // Start is called before the first frame update
    void Start()
    {
        m_achivement = GameObject.FindObjectOfType<AchivementTracker>();
        m_achivement.UnlockAchivement(AchivementID);
    }
}
