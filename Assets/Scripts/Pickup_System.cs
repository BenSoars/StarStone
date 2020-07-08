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

    public List<GameObject> clockPart = new List<GameObject>();

    public bool m_spawnNote;
    public bool m_spawnCogs;

    public Transform cameraLook;

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

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_clockHit, 6f))
        {
            if(m_clockHit.collider.gameObject.name == "Steampunk Clock")
            {
                Debug.Log("YEET");
            }

            if(m_clockHit.collider.gameObject.name == "Cog 1(Clone)" && Input.GetKeyDown("f"))
            {
                m_clockHit.collider.gameObject.transform.parent = floatPoint.transform;
                m_clockHit.collider.gameObject.transform.localPosition = Vector3.zero;
                m_clockHit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                m_clockHit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
