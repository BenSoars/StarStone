using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//Kurtis Watson
public class Pickup_System : MonoBehaviour
{
    public List<Transform> m_locations = new List<Transform>();

    private Transform m_desiredLocation;

    public GameObject m_note;
    private int m_clockPart;

    public GameObject floatPoint;
    public GameObject currentPart;

    public List<GameObject> clockPart = new List<GameObject>();

    public bool m_spawnNote;
    public bool m_spawnCogs;
    private bool itemHeld;
    private bool isRepairing;

    public int currentPartID;

    public Transform cameraLook;

    private Clock_Controller clockController;
    private User_Interface userInterface;

    public float currentRepairTime;
    public float repairTime;


    private GameObject weaponHand;
    private GameObject meleeHand;
    private GameObject repairHands;

    private Animator animator;

    public bool clockFixed;

    private int repairedParts;

    private void Start()
    {
        animator = gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).GetComponent<Animator>();

        //weaponHand = GameObject.Find("Arm_Position");
        meleeHand = GameObject.Find("Main Weapons");
        repairHands = GameObject.Find("Repair Hands");
        repairHands.active = false;

        
        Debug.Log("Animator: " + animator.gameObject);

        clockController = FindObjectOfType<Clock_Controller>();
        userInterface = FindObjectOfType<User_Interface>();
    }

    private void Update()
    {
        f_spawnPickup();
        f_clockRepair();
    }

    void f_spawnPickup()
    {
        int random = Random.Range(0, m_locations.Count);
        m_desiredLocation = m_locations[random];
        if (m_spawnNote == true)
        {
            userInterface.f_popupText();
            Instantiate(m_note, m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
        }
        if (m_spawnCogs == true)
        {
            userInterface.f_popupText();
            Instantiate(clockPart[m_clockPart], m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
            m_clockPart += 1;
        }
    }

    void f_clockRepair()
    {
        RaycastHit m_clockHit;

        animator.SetBool("Repairing", isRepairing); //Activate the animator.

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_clockHit, 100f))
        {
            float closeEnough = Vector3.Distance(transform.position, m_clockHit.collider.gameObject.transform.position); //Check distance between player and scene objects.
            if (Input.GetKeyDown("f") && m_clockHit.collider.gameObject.name != "Steampunk Clock" && itemHeld == true) //Drop clock part.
            {
                itemHeld = false;
                currentPart.transform.parent = null; //Unparent the picked up item from the player so it can be dropped.
                currentPart.GetComponent<Rigidbody>().isKinematic = false;
                currentPart.GetComponent<BoxCollider>().enabled = true;

                meleeHand.active = true; //Enable to player to be able to melee.
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
                
                meleeHand.active = false; //Disable player from being able to melee.
                repairHands.active = true; //Switch player hands to carrying the object.          
            }

            if (m_clockHit.collider.gameObject.name == "Steampunk Clock" && Input.GetKey("f") && itemHeld == true && closeEnough <= 4) //Repair the clock.
            { 
                isRepairing = true; //Activate animation.
                currentRepairTime += Time.deltaTime; //Increase the current repair time if the player is holding 'F'.
                userInterface.repairBar.active = true; //Enable the repair bar so the player can see repair progress.

                if (currentRepairTime >= repairTime)
                {
                    Destroy(currentPart); //Remove the held gameobject after it has been added to the clock.
                    itemHeld = false;
                    repairedParts += 1; //Increase the clock repaired parts by 1 to check for if the player can set time.
                    
                    isRepairing = false;
                    currentRepairTime = 0;                                      
                    userInterface.repairBar.active = false;
                    clockController.clockParts[currentPartID].active = true;

                    meleeHand.active = true;
                    repairHands.active = false;

                    if(repairedParts == 5) //Check if all parts have been added >
                    {
                        clockFixed = true; //if so it will allow the player to begin adjusting the clocks time.
                    }
                }
            }
            else
            {
                isRepairing = false;
                currentRepairTime = 0; //Reset current repair time.
                userInterface.repairBar.active = false; //Hide repair bar on the UI.
            }
        }
    }
}
