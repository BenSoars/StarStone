using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//Kurtis Watson
public class Pickup_System : MonoBehaviour
{
    [Header("Pickup Mechanics")]
    [Space(2)]
    private Transform m_desiredLocation;
    private List<Transform> m_locations = new List<Transform>();   
    public GameObject floatPoint;
    public GameObject currentPart;
    public bool spawnNote;
    public bool spawnCogs;
    public bool itemHeld;
    private bool m_isRepairing;
    public int currentPartID;
    public Animator animator;
    public Transform camera;

    [Header("Script References")]
    [Space(2)]
    private Clock_Controller m_clockController;
    private User_Interface m_userInterface;
    public Prototype_Classes prototypeClasses;

    public float currentRepairTime;
    public float repairTime;

    [Header("Player Attributes")]
    [Space(2)]
    public GameObject weaponHands;
    public GameObject repairHands;

    [Header("Clock Mechanics")]
    [Space(2)]
    public bool clockFixed;
    private int m_clockPart;
    public List<GameObject> clockPart = new List<GameObject>();
    private int m_repairedParts;
    public GameObject note;
    private int m_noteID;

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
                    m_note.GetComponent<Note>().NoteName = "21.06.1923";
                    m_note.GetComponent<Note>().NoteText = "We should not have touched it. Oh God, whoever that is anymore. If only we could go back. Only a few weeks ago everything was normal. It was only until those damned power-hungry scientists went rogue and started poking around the new generator they started building.";
                    break;
                case 2:
                    m_note.GetComponent<Note>().NoteName = "24.06.1923";
                    m_note.GetComponent<Note>().NoteText = "We have just discovered a new part of the temple. It seems to be some sort of sacrificial pit? It reeks with blood. The other scientists are already planning on renovation, they think that the old sacrifices are the key to the power. I feel like this is bad. They want the star stones to power everything in sight.";
                    break;
                case 3:
                    m_note.GetComponent<Note>().NoteName = "26.06.1923";
                    m_note.GetComponent<Note>().NoteText = "Time: " + m_clockController.globalHour + ":" + m_clockController.globalMin + " - Ruins have been check and the generator is in full working order. Fuel consumption gathered from the Temples core is at a steady rate and the stones are charging as expected.";
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
                    m_repairedParts += 1; //Increase the clock repaired parts by 1 to check for if the player can set time.
                    
                    m_isRepairing = false;
                    currentRepairTime = 0;                                      
                    m_userInterface.repairBar.active = false;
                    m_clockController.clockParts[currentPartID].active = true;

                    weaponHands.active = true;
                    repairHands.active = false;

                    if(m_repairedParts == 5) //Check if all parts have been added >
                    {
                        clockFixed = true; //if so it will allow the player to begin adjusting the clocks time.
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
