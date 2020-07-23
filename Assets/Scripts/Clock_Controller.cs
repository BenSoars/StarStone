using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Kurtis Watson
public class Clock_Controller : MonoBehaviour
{
    private Animator m_animator;

    public Camera camera;

    private Pickup_System pickupSystem;
    public Gun_Generic gunGeneric;

    public string globalMin;
    public string globalHour;

    private int m_desiredMin;
    private int m_desiredHour;

    private int m_currentMin;
    private int m_currentHour;

    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject player;
    public GameObject portal;

    private int smallRotation = 0;
    private int bigRotation = 0;

    private bool m_isActive;

    public bool canShoot;

    public List<GameObject> clockParts = new List<GameObject>(); 

    private void Start()
    {
        pickupSystem = FindObjectOfType<Pickup_System>();
        portal.active = false;
        for (int i = 0; i < clockParts.Count; i++)
        {
            clockParts[i].active = false;
        }
        m_animator = GetComponentInChildren<Animator>();
        m_desiredMin = UnityEngine.Random.Range(1, 11);
        m_desiredHour = UnityEngine.Random.Range(1, 11);

        if (m_desiredHour < 10)
        {
            globalHour = "0" + m_desiredHour;
        }
        else globalHour = m_desiredHour.ToString();

        if (m_desiredMin == 12)
        {
            globalMin = "12";
        }
        else globalMin = "" + m_desiredMin * 5;

        Debug.Log("Hour: " + m_desiredHour + " Min: " + m_desiredMin);
    }

    // Update is called once per frame
    void Update()
    {
        f_clockMechanic();
        f_checkTime();      
    }

    void f_clockMechanic()
    {
        m_animator.SetBool("Active", m_isActive);
        f_checkTime();


        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("gearsTurning") && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            portal.active = true;
        }
        RaycastHit m_clockHit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_clockHit, 4f))
        {
            if (m_clockHit.collider.gameObject.name == "Steampunk Clock") //Repair the clock.
            {
                canShoot = false;
            }
            else canShoot = true;
        }
        

        if (Vector3.Distance(transform.position, player.transform.position) < 3 && pickupSystem.clockFixed == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && m_isActive == false)
            {
                smallRotation += 30;
                m_currentHour += 1;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && m_isActive == false)
            {
                bigRotation += 30;
                m_currentMin += 1;
            }

            if (m_currentHour == 12)
            {
                m_currentHour = 0;
            }

            if (m_currentMin == 12)
            {
                m_currentMin = 0;
            }

            hourHand.transform.rotation = Quaternion.Euler(hourHand.transform.rotation.eulerAngles.x, hourHand.transform.rotation.eulerAngles.y, smallRotation);
            minuteHand.transform.rotation = Quaternion.Euler(minuteHand.transform.rotation.eulerAngles.x, minuteHand.transform.rotation.eulerAngles.y, bigRotation);
        }
    }

    void f_checkTime()
    {
        if(m_desiredHour == m_currentHour && m_desiredMin == m_currentMin)
        {
            canShoot = true;
            m_isActive = true;
            Debug.Log("Time is correct");
        }
        else
        {
            m_isActive = false;
        }
    }
}
