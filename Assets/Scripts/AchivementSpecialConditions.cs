using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementSpecialConditions : MonoBehaviour
{
    // Ben Soars
   

    private AchivementTracker m_achivement; // the achivement tracker system

    public int KilledEnemies; // used for the killstreak achivements
    private float timerBetweenKills; // timer check between kills for kill streak achivements
    private bool perfectRunCheck; // bool check for the perfect run achivement
    private float timerRoundCheck; // timer check for the seed run achivement

    // Start is called before the first frame update
    void Start()
    {
        timerRoundCheck = -1; // make sure the timer check doesn't start on scene start
        m_achivement = GameObject.FindObjectOfType<AchivementTracker>(); // 
    }

    // Update is called once per frame
    public void CheckForKillAchivements()
    {
        if (m_achivement) // if it can find an achivement tracker
        {
            // killstreak achivement checker
            switch (KilledEnemies)
            {
                case (2): // if two were killed quickly
                    m_achivement.UnlockAchivement(3);
                    break;

                case (3): // if three were killed quickly
                    m_achivement.UnlockAchivement(4);
                    break;

                case (4): // if four were killed quickly
                    m_achivement.UnlockAchivement(5);
                    break;
            }
            
            // total kills achivement checker
            switch (PlayerPrefs.GetInt("TotalKills")) // check the total amount of saved kills
            {
                case (10): // if 10
                    m_achivement.UnlockAchivement(6);
                    //unlock the total 10 kills
                    break;
                case (50): // if 50
                    m_achivement.UnlockAchivement(7);
                    //unlock the total 50 kills
                    break;
                case (100): // if 100
                    m_achivement.UnlockAchivement(8);
                    //unlock the total 100 kills
                    break;
                case (250): // if 250
                    m_achivement.UnlockAchivement(9);
                    //unlock the total 250 kills
                    break;
                case (500): // if 500
                    m_achivement.UnlockAchivement(10);
                    //unlock the total 500 kills
                    break;
            }
            
        }
    }

    public void CheckForRoundAchivements()
    {
        // run at the end of a round
        if (m_achivement) // if it can find an achivement tracker
        {
            if (timerRoundCheck > 0) // if the timer check is greater than 0
            {
                m_achivement.UnlockAchivement(13); // success, unlock the achivement for speed run
            }

            if (perfectRunCheck == true) // if the player hasn't been hit once
            {
                m_achivement.UnlockAchivement(12); // success, unlock the achivement for a perfect run
            }
        }

        perfectRunCheck = true; // reset the perfect chack so the play can try again next round
    }

    void Update()
    {
        if (timerBetweenKills >= 0) // if the timer is greater than or equal to 0
        {
            timerBetweenKills -= Time.deltaTime; // start decreasing the timer back down to zero
        } else
        {
            KilledEnemies = 0; // reset the enemys since the timer has reset and the player took too long
        }

        if (timerRoundCheck >= 0) // check the timer
        {
            timerRoundCheck -= Time.deltaTime;
            Debug.Log("timer round: " + timerRoundCheck.ToString());
        }
        
    }

    public void setRoundChecker() // called in wave system to reset the timer on a round start
    {
        timerRoundCheck = 60;
    }

    public void setKillTimer() // called in the enemy script to reset the timer
    {
        timerBetweenKills = 1;
    }

    public void imperfectRun() // called when enemy hits the player
    {
        perfectRunCheck = false;
    
    }
}
