using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Kurtis Watson
public class Clock_Controller : MonoBehaviour
{
    [Header("Required Components")]
    [Space(2)]
    private Animator m_animator;
    public Camera camera;
    public GameObject player;
    private Pickup_System m_pickupSystem;
    public Gun_Generic gunGeneric;
    public bool canAim;
    public bool canShoot;
    private bool m_isActive;

    [Header("Time Values")]
    [Space(2)]
    public string globalMin;
    public string globalHour;
    private int m_desiredMin;
    private int m_desiredHour;
    private int m_currentMin;
    private int m_currentHour;

    [Header("Clock Mechanics")]
    [Space(2)]
    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject portal;
    private int m_smallRotation = 0;
    private int m_bigRotation = 0;
    public List<GameObject> clockParts = new List<GameObject>(); 

    private void Start()
    {
        m_pickupSystem = FindObjectOfType<Pickup_System>();
        m_animator = GetComponentInChildren<Animator>();

        m_desiredMin = UnityEngine.Random.Range(1, 11); //Generate a random minute for the time to set.
        m_desiredHour = UnityEngine.Random.Range(1, 11);

        f_setGlobalTime();
         
        for (int i = 0; i < clockParts.Count; i++) //Disable clock parts object so the player cannot see them.
        {
            clockParts[i].active = false;
        }

        portal.active = false; //Hide the portal object.

        
    }

    // Update is called once per frame
    void Update()
    {
        f_clockMechanic();
        f_checkTime();
        if(SceneManager.GetActiveScene().name == "Main_Menu")
        {
            m_animator.SetBool("Active", true);
        }      
    }

    void f_setGlobalTime() //Set the time for the notes in a readable format.
    {
        if (m_desiredHour < 10) //To make the clock time show correctly on the notes in a readable format, these check allow for that time to be shown correctly; even though the backend code works differently.
        {
            globalHour = "0" + m_desiredHour; //Rather than showing as "9:2" it will show as "09:10".
        }
        else globalHour = m_desiredHour.ToString();

        if (m_desiredMin == 12)
        {
            globalMin = "12";
        }
        else globalMin = "" + m_desiredMin * 5;

        if(m_desiredMin == 1)
        {
            globalMin = "05";
        }
    }

    void f_clockMechanic()
    {
        m_animator.SetBool("Active", m_isActive); //Update the animator based on m_isActive value.
        f_checkTime();

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("gearsTurning") && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) //Activate portal at the end of the animation.
        {
            portal.active = true;
        }

        RaycastHit m_clockHit; //Create a raycast.
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_clockHit, 4f)) //If the raycase hits an object within a distance of 4 >
        {
            if (m_clockHit.collider.gameObject.name == "Steampunk Clock") //and the name is 'Steampunk Clock' >
            {
                canAim = false;
                canShoot = false; //then stop the player from shooting their gun (allow them to change clock time). 
            }
            else canAim = true; canShoot = true;
        }
        

        if (Vector3.Distance(transform.position, player.transform.position) < 3 && m_pickupSystem.clockFixed == true) //Check for if the clock is repaired and if the player is close enough.
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && m_isActive == false)
            {
                m_smallRotation += 30; //Rotate clock arm to hours around the clock upon clicking the mouse.
                m_currentHour += 1; //Add a value to the current ho
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && m_isActive == false)
            {
                m_bigRotation += 30;
                m_currentMin += 1;
            }

            if (m_currentHour == 12) //Set value to zero at 12 o clock.
            {
                m_currentHour = 0;
            }

            if (m_currentMin == 12)
            {
                m_currentMin = 0;
            }

            hourHand.transform.rotation = Quaternion.Euler(hourHand.transform.rotation.eulerAngles.x, hourHand.transform.rotation.eulerAngles.y, m_smallRotation); //Set rotation of the clock hands.
            minuteHand.transform.rotation = Quaternion.Euler(minuteHand.transform.rotation.eulerAngles.x, minuteHand.transform.rotation.eulerAngles.y, m_bigRotation);
        }
    }

    void f_checkTime() //Check if the time the player has set is correct.
    {
        if(m_desiredHour == m_currentHour && m_desiredMin == m_currentMin)
        {
            canShoot = true;
            m_isActive = true; //Sets the bool true so the active animation can begin on the clock as the time has been set correctly.
            Debug.Log("Time is correct");
        }
        else
        {
            m_isActive = false;
        }
    }
}
