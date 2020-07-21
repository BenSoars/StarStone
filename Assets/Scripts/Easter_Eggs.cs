﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easter_Eggs : MonoBehaviour
{
    public int runesFound;
    public Transform camera;

    private bool musicPlayed;

    public AudioSource audioSource;
    public AudioClip song;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        f_runesEasterEgg();
    }

    void f_runesEasterEgg()
    {
        RaycastHit m_objectHit;

        if (Physics.Raycast(camera.position, camera.forward, out m_objectHit, 3f) && Input.GetKeyDown("f"))
        {
            float distance = Vector3.Distance(camera.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.name == "Rune") && distance <= 3 && Input.GetKeyDown("f"))
            {
                m_objectHit.collider.gameObject.GetComponentInChildren<Rune_Controller>().f_runeEasterEgg();
            }
        }

        Debug.Log("Runes Found: " + runesFound);
        if(runesFound == 8 && musicPlayed == false)
        {
            musicPlayed = true;
            audioSource.clip = song;
            audioSource.Play();
        }
    }
}
