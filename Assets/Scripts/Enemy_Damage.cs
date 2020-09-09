using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damage : MonoBehaviour
{
    
    // Ben Soars

    public float m_damage; // the damage the attack deals, is altered from the enemy controller
    public BoxCollider m_hurtBox; // the hurtbox collider
    private AchievementSpecialConditions m_SpecialTracker;
    public Player_Controller r_player;
    public bool damaged;
    // Start is called before the first frame update

    void OnEnable() // when enabled renable the hurtbox if it's disabled
    {
        m_SpecialTracker = GameObject.FindObjectOfType<AchievementSpecialConditions>();
        m_hurtBox.enabled = true; // re-enable the box collider when it's enabled, as it could be turned off from the last use
    }

    void OnTriggerStay (Collider other) // if it enters a player
    {
        if (other.CompareTag("Player"))
        {
            r_player = other.gameObject.GetComponent<Player_Controller>(); // get player componenet
            r_player.playerHealth -= m_damage * r_player.defenceValue; // take health away based on damage and defence
            m_hurtBox.enabled = false; // disable the hurtbox to prevent multiple hits per frames
            r_player.audio.playPlayerHurt(); // play hurt noise
            m_SpecialTracker.imperfectRun(); // player was it, set run to be imperfect
            StartCoroutine("bloodEffect");
        }
    }

    private IEnumerator bloodEffect()
    {
        damaged = true;
        yield return new WaitForSeconds(0.5f);
        damaged = false;
    }
}
