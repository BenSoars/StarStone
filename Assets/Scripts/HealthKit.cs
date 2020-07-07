using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    // Ben Soars
    public int m_healthValue; // the amount to heal
    public AudioClip pickupSound; // the sound that will play when picked up

    private Audio_System m_audio;

    void Start()
    {
        m_audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // if player walks into it
        {
            other.GetComponent<Player_Controller>().playerHealth += m_healthValue; //add health to player
            m_audio.playOther(pickupSound); // play pickup sound
            Destroy(gameObject); // destroy self so can't be picked up again
            
        }
    }
}
