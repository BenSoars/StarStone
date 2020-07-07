using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//Kurtis Watson
public class Notes_System : MonoBehaviour
{
    public List<Transform> m_locations = new List<Transform>();

    private Transform m_desiredLocation;

    private Wave_System m_waveSystem;

    public GameObject m_note;

    public bool m_spawnNote;

    private void Start()
    {
        m_waveSystem = FindObjectOfType<Wave_System>();       
    }
    private void Update()
    {
        if (m_spawnNote == true)
        {
            m_spawnNote = false;
            int random = Random.Range(0, m_locations.Count); //Pick random number from 0 to total desirable locations.

            m_desiredLocation = m_locations[random]; //Set the desired location based on random number.

            Instantiate(m_note, m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
        }
    }
}
