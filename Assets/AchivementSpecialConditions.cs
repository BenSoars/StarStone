using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementSpecialConditions : MonoBehaviour
{
    // Ben Soars
   
    private AchivementTracker m_achivement;

    public int KilledEnemies; // used for the killstreak achivements
    private float timerBetweenChecks;
    private bool perfectRunCheck;
    private float timerRoundCheck;

    // Start is called before the first frame update
    void Start()
    {
        timerRoundCheck = -1;
        m_achivement = GameObject.FindObjectOfType<AchivementTracker>();
    }

    // Update is called once per frame
    public void CheckForKillAchivements()
    {
        if (m_achivement) // if it can find an achivement tracker
        {
            // killstreak achivement checker
            switch (KilledEnemies)
            {
                case (2):
                    m_achivement.UnlockAchivement(3);
                    break;

                case (3):
                    m_achivement.UnlockAchivement(4);
                    break;

                case (4):
                    m_achivement.UnlockAchivement(5);
                    break;
            }
            
            switch (PlayerPrefs.GetInt("TotalKills"))
            {
                case (10):
                    m_achivement.UnlockAchivement(6);
                    break;
                case (50):
                    m_achivement.UnlockAchivement(7);
                    break;
                case (100):
                    m_achivement.UnlockAchivement(8);
                    break;
                case (250):
                    m_achivement.UnlockAchivement(9);
                    break;
                case (500):
                    m_achivement.UnlockAchivement(10);
                    break;
            }
            
        }
    }

    public void CheckForRoundAchivements()
    {
        if (m_achivement) // if it can find an achivement tracker
        {
            if (timerRoundCheck > 0)
            {
                m_achivement.UnlockAchivement(13);
            }

            if (perfectRunCheck == true)
            {
                m_achivement.UnlockAchivement(12);
            }
        }

        perfectRunCheck = true;
    }

    void Update()
    {
        if (timerBetweenChecks >= 0)
        {
            timerBetweenChecks -= Time.deltaTime;
        } else
        {
            KilledEnemies = 0;
        }

        if (timerRoundCheck >= 0)
        {
            timerRoundCheck -= Time.deltaTime;
            Debug.Log("timer round: " + timerRoundCheck.ToString());
        }
        
    }

    public void setRoundChecker()
    {
        timerRoundCheck = 60;
    }

    public void setKillTimer()
    {
        timerBetweenChecks = 1;
    }

    public void imperfectRun()
    {
        perfectRunCheck = false;
        Debug.Log("Player was hit, Imperfect run");
    }
}
