using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Replenish : MonoBehaviour
{
    // Ben Soars
    public string GunType = "ALL";
    public int AmmoWorth;
    public AudioClip pickupSound; // the sound that will play when picked up

    private Audio_System m_audio;

    void Start()
    {
        m_audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // when the player walks over it
        {
            Gun_Generic Gun = FindObjectOfType<Gun_Generic>();

            if (Gun) // if the player has a gun equiped
            {
                Debug.Log("found gun, " + Gun);

                if (Gun.m_name == GunType)
                {
                    Gun.m_maxAmmo += AmmoWorth; // increase the ammo count
                    Debug.Log("was correct Gun");
                    
                    Gun.f_updateUI(); // update the UI so it reflects the current amount
                    Destroy(gameObject); // destroy the ammo so it can't be infinate

                    m_audio.playOther(pickupSound); // play pickup sound
                } else
                {
                    Debug.Log(Gun.m_name + " was not the same gun type as " + GunType);
                }
            }
        }
    }
}
