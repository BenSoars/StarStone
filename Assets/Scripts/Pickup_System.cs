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
    public int currentPartID;

    public Transform cameraLook;

    private Clock_Controller clockController;
    private User_Interface userInterface;

    public float currentRepairTime;
    public float repairTime;

    private void Start()
    {
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

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_clockHit, 100f))
        {
            float closeEnough = Vector3.Distance(transform.position, m_clockHit.collider.gameObject.transform.position);
            if (Input.GetKeyDown("f") && m_clockHit.collider.gameObject.name != "Steampunk Clock" && itemHeld == true)
            {
                Debug.Log("Dropped");
                itemHeld = false;
                currentPart.transform.parent = null;
                currentPart.GetComponent<Rigidbody>().isKinematic = false;
                currentPart.GetComponent<BoxCollider>().enabled = true;
            }

            if (m_clockHit.collider.gameObject.layer == 13 && Input.GetKeyDown("f") && itemHeld == false && closeEnough <= 3)
            {
                currentPart = m_clockHit.collider.gameObject;
                currentPartID = currentPart.GetComponent<Clock_ID>().clockPartID;
                Debug.Log("ID: " + currentPartID);
                itemHeld = true;
                currentPart.transform.parent = floatPoint.transform;
                currentPart.transform.localPosition = Vector3.zero;
                currentPart.GetComponent<Rigidbody>().isKinematic = true;
                currentPart.GetComponent<BoxCollider>().enabled = false;
            }

            if (m_clockHit.collider.gameObject.name == "Steampunk Clock" && Input.GetKey("f") && itemHeld == true && closeEnough <= 4)
            {
                currentRepairTime += Time.deltaTime;
                userInterface.repairBar.active = true;

                if (currentRepairTime >= repairTime)
                {
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
                currentRepairTime = 0;
                userInterface.repairBar.active = false;
            }
        }
    }
}
