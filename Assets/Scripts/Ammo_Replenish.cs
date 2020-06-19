using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Replenish : MonoBehaviour
{
    // Ben Soars
    public string GunType = "ALL";
    public int AmmoWorth;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gun_Generic Gun = FindObjectOfType<Gun_Generic>();
            if (Gun)
            {
                m_currentAmmo += other.GetComponent<Ammo_Replenish>().AmmoWorth;
                if (m_currentAmmo > m_maxAmmo) { m_currentAmmo = m_maxAmmo; }
                Destroy(other.gameObject);
                f_updateUI();
            }
        }
    }
}
