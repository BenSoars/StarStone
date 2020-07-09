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

    private void Start()
    {
        animator = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).GetComponent<Animator>();

        weaponHand = GameObject.Find("Arm_Position");
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
            m_spawnNote = false;
            Instantiate(m_note, m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
        }

        if (m_spawnCogs == true)
        {
            m_spawnCogs = false;
            Instantiate(clockPart[m_clockPart], m_desiredLocation.position, Quaternion.identity); //Instantiate the note at the chosen location.
            m_clockPart += 1;
        }
    }

    void f_clockRepair()
    {
        RaycastHit m_clockHit;

        animator.SetBool("Repairing", isRepairing);

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_clockHit, 100f))
        {
            float closeEnough = Vector3.Distance(transform.position, m_clockHit.collider.gameObject.transform.position);
            if (Input.GetKeyDown("f") && m_clockHit.collider.gameObject.name != "Steampunk Clock" && itemHeld == true) //Drop clock part.
            {
                Debug.Log("Dropped");
                itemHeld = false;
                currentPart.transform.parent = null;
                currentPart.GetComponent<Rigidbody>().isKinematic = false;
                currentPart.GetComponent<BoxCollider>().enabled = true;
                weaponHand.active = true;
                meleeHand.active = true;
                repairHands.active = false;
            }

            if (m_clockHit.collider.gameObject.layer == 13 && Input.GetKeyDown("f") && itemHeld == false && closeEnough <= 3) //Hold the clock part.
            {                
                weaponHand.active = false;
                meleeHand.active = false;
                repairHands.active = true;
                currentPart = m_clockHit.collider.gameObject;
                currentPartID = currentPart.GetComponent<Clock_ID>().clockPartID;
                Debug.Log("ID: " + currentPartID);
                itemHeld = true;
                currentPart.transform.parent = floatPoint.transform;
                currentPart.transform.localPosition = Vector3.zero;
                currentPart.GetComponent<Rigidbody>().isKinematic = true;
                currentPart.GetComponent<BoxCollider>().enabled = false;              
            }

            if (m_clockHit.collider.gameObject.name == "Steampunk Clock" && Input.GetKey("f") && itemHeld == true && closeEnough <= 4) //Repair the clock.
            { 
                isRepairing = true;
                currentRepairTime += Time.deltaTime;
                userInterface.repairBar.active = true;

                if (currentRepairTime >= repairTime)
                {
                    weaponHand.active = true;
                    meleeHand.active = true;
                    repairHands.active = false;
                    isRepairing = false;
                    currentRepairTime = 0;
                    Destroy(currentPart);
                    itemHeld = false;
                    userInterface.repairBar.active = false;
                    clockController.clockParts[currentPartID].active = true;
                }

                //Debug.Log("Current Repair: " + currentRepairTime);
            }
            else
            {
                isRepairing = false;
                currentRepairTime = 0;
                userInterface.repairBar.active = false;
            }
        }
    }
}
