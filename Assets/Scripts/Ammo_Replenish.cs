using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Replenish : MonoBehaviour
{
    // Ben Soars
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
            Weapon_Switch weapons = FindObjectOfType<Weapon_Switch>();

            for (int i = 0; i < 2; i++)
            {
                Gun_Generic Gun = weapons.m_Weapons[i].GetComponent<Gun_Generic>();
                if (Gun) // if the player has a gun equiped
                {

                    Gun.m_maxAmmo += AmmoWorth; // increase the ammo count


                    Gun.f_updateUI(); // update the UI so it reflects the current amount
                    Destroy(gameObject); // destroy the ammo so it can't be infinate

                    m_audio.playOther(pickupSound); // play pickup sound
                }
            }
        }
    }
}
