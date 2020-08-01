using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Replenish : MonoBehaviour
{
    // Ben Soars
    public int AmmoWorth; // the amount of ammo that pack is worth
    public AudioClip pickupSound; // the sound that will play when picked up

    private Audio_System m_audio; // the audio system that will play on pickup

    void Start()
    {
        m_audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // when the player walks over it
        {
            Weapon_Switch weapons = FindObjectOfType<Weapon_Switch>();

            for (int i = 0; i < weapons.m_Weapons.Count - 1; i++) // for loop through all the weapons that can have ammo, in this case there are only two
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
