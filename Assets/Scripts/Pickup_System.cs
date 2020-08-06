﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//Kurtis Watson
public class Pickup_System : MonoBehaviour
{    
    [Header("Note Components")]    
    [Tooltip("Set the date for the note.")]
    public string[] noteDate;
    [Tooltip("Set the text for the note.")]
    public string[] noteText;

    [Header("Pickup Mechanics")]
    [Tooltip("Location where the picked up part will float.")]
    public GameObject floatPoint;
    private Transform m_desiredLocation; //Location to spawn object.
    private List<Transform> m_locations = new List<Transform>(); //List of all posisible locations. 
    [Tooltip("Current held object.")]
    public GameObject currentPart; //Current part held by player.
    [Tooltip("Spawn a note.")]
    public bool spawnNote; //Spawn a note if true.
    [Tooltip("Spawn a clock part.")]
    public bool spawnCogs; //Spawn a clock part if true.
    [Tooltip("Check if the player is holding an item.")]
    public bool itemHeld; //Check if the player is holding a clock part.
    private bool m_isRepairing; //Used to check if the clock is being repaired.
    [Tooltip("Grabs the ID of the held current part so that the clock can show the correct part after repair.")]
    public int currentPartID; //Grabs current part ID held by the player.
    public Animator animator; //Grabs animator of repair mechanic.
    public Transform camera; //Reference the camera attached to the player.

    [Header("Script References")]
    [Space(2)]
    private Clock_Controller m_clockController; //Clock controller reference.
    private User_Interface m_userInterface; //User interface reference.
    public Prototype_Classes prototypeClasses; //Prototype classes reference.

    [Tooltip("Set how long it takes for the player to repair the clock.")]
    public float repairTime; //How long it takes to repair the clock.
    public float currentRepairTime; //Current repair time.
    
    [Header("Player Attributes")]
    [Space(2)]
    [Tooltip("Weapons game object.")]
    public GameObject weaponHands;
    [Tooltip("Repair hand game object.")]
    public GameObject repairHands;

    [Header("Clock Mechanics")]
    [Space(2)]
    [Tooltip("Check for if all parts have been repaired on the clock.")]
    public bool clockFixed; //True/false clock fixed.
    private int m_clockPart; //Store the int of the current clock part.
    [Tooltip("List of all the clock parts to spawn.")]
    public List<GameObject> clockPart = new List<GameObject>(); //List of all the clock parts.
    [Tooltip("Sum of how many parts have been added to the clock.")]
    public int repairedParts;
    [Tooltip("Note game object.")]
    public GameObject note;
    private int m_noteID; //Note ID set for text value.

    private AchievementTracker m_achivement;

    private void Start()
    {
        //animator = gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).GetComponent<Animator>(); //Get the animator of the hands.
        repairHands.active = false; //Disable player being able to see repair hands.

        for (int i = 1; i < 19; i++) //Add all the spawnpoints to a list.
        {
            m_locations.Add(GameObject.Find("NoteLocation_" + i).transform);
        }

        m_clockController = FindObjectOfType<Clock_Controller>(); //Reference the required scripts.
        m_userInterface = FindObjectOfType<User_Interface>();
        m_achivement = FindObjectOfType<AchievementTracker>();
    }

    private void Update()
    {
        f_spawnPickup();
        f_clockRepair();
    }

    void f_spawnPickup()
    {
        int random = Random.Range(0, m_locations.Count); //Choose a random location to spawn the note.
        m_desiredLocation = m_locations[random]; //Set the desired location from a random value in the list.
        if (spawnNote == true)
        {
            m_userInterface.f_popupText(); //Indicate to the player that a note has spawned.
            GameObject m_note = Instantiate(note, m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
            m_noteID += 1;
            switch (m_noteID) {
                case 1:
                    m_note.GetComponent<Note>().NoteName = noteDate[0];
                    m_note.GetComponent<Note>().NoteText = noteText[0];
                    break;
                case 2:
                    m_note.GetComponent<Note>().NoteName = noteDate[1];
                    m_note.GetComponent<Note>().NoteText = noteText[1];
                    break;
                case 3:
                    m_note.GetComponent<Note>().NoteName = noteDate[2];
                    m_note.GetComponent<Note>().NoteText = "Time: " + m_clockController.globalHour + ":" + m_clockController.globalMin + noteText[2];
                    break;
            }
            
        }
        if (spawnCogs == true)
        {
            m_userInterface.f_popupText(); //Indicate to the player that a clock part has spawned.
            Instantiate(clockPart[m_clockPart], m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
            m_clockPart += 1;
        }
    }

    void f_clockRepair()
    {
        RaycastHit m_clockHit;

        animator.SetBool("Repairing", m_isRepairing); //Activate the animator.

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_clockHit, 100f))
        {
            float closeEnough = Vector3.Distance(transform.position, m_clockHit.collider.gameObject.transform.position); //Check distance between player and scene objects.
            if (Input.GetKeyDown("f") && m_clockHit.collider.gameObject.name != "Steampunk Clock" && itemHeld == true) //Drop clock part.
            {
                itemHeld = false;
                currentPart.transform.parent = null; //Unparent the picked up item from the player so it can be dropped.
                currentPart.GetComponent<Rigidbody>().isKinematic = false;
                currentPart.GetComponent<BoxCollider>().enabled = true;
                currentPart.GetComponent<Clock_ID>().pickedUp = false;

                weaponHands.active = true; //Enable to player to be able to melee.
                repairHands.active = false; //Switch back to weapon classes.
            }

            if (m_clockHit.collider.gameObject.layer == 13 && Input.GetKeyDown("f") && itemHeld == false && closeEnough <= 3) //Pickup the clock part.
            {
                itemHeld = true; //Bool to enable the player to be able to drop the item.

                currentPart = m_clockHit.collider.gameObject; //Grab the current gameobject the player was looking at and set it as the current held part.
                currentPartID = currentPart.GetComponent<Clock_ID>().clockPartID; //Grab the ID of the object and store it in a variable.

                currentPart.transform.parent = floatPoint.transform; //Move the picked up object to between the players hands (carrying the item).
                currentPart.transform.localPosition = Vector3.zero;
                currentPart.GetComponent<Rigidbody>().isKinematic = true;
                currentPart.GetComponent<BoxCollider>().enabled = false;
                currentPart.GetComponent<Clock_ID>().pickedUp = true;

                weaponHands.active = false; //Disable player from being able to melee.
                repairHands.active = true; //Switch player hands to carrying the object.          
            }

            if (m_clockHit.collider.gameObject.name == "Steampunk Clock" && itemHeld == true && closeEnough <= 4 && Input.GetKey(KeyCode.Mouse0)) //Repair the clock.
            {
                m_isRepairing = true; //Activate animation.
                currentRepairTime += Time.deltaTime; //Increase the current repair time if the player is holding 'F'.
                m_userInterface.repairBar.active = true; //Enable the repair bar so the player can see repair progress.

                if (currentRepairTime >= repairTime)
                {
                    Destroy(currentPart); //Remove the held gameobject after it has been added to the clock.
                    itemHeld = false;
                    repairedParts += 1; //Increase the clock repaired parts by 1 to check for if the player can set time.

                    m_isRepairing = false;
                    currentRepairTime = 0;
                    m_userInterface.repairBar.active = false;
                    m_clockController.clockParts[currentPartID].active = true;

                    weaponHands.active = true;
                    repairHands.active = false;

                    if (repairedParts == 5) //Check if all parts have been added >
                    {
                        clockFixed = true; //if so it will allow the player to begin adjusting the clocks time.
                        if (m_achivement)
                        {
                            m_achivement.UnlockAchievement(14);
                        }
                    }
                }
            }

            else
            {
                m_isRepairing = false;
                currentRepairTime = 0; //Reset current repair time.
                m_userInterface.repairBar.active = false; //Hide repair bar on the UI.
            }
        }
    }
}
