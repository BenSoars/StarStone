using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damage : MonoBehaviour
{
    // Ben Soars

    public float m_damage; // the damage the attack deals, is altered from the enemy controller
    public BoxCollider m_hurtBox; // the hurtbox collider

    public Player_Controller r_player;
    // Start is called before the first frame update
    
    void OnEnable() // when enabled renable the hurtbox if it's disabled
    {
        m_hurtBox.enabled = true;
    }

    void OnTriggerStay (Collider other) // if it enters a player
    {
        if (other.CompareTag("Player"))
        {
            r_player = other.gameObject.GetComponent<Player_Controller>(); // get player componenet
            r_player.playerHealth -= m_damage * r_player.defenceValue; // take health away based on damage and defence
            m_hurtBox.enabled = false; // disable the hurtbox to prevent multiple hits per frames]
            r_player.audio.playPlayerHurt();
        }
    }
}
