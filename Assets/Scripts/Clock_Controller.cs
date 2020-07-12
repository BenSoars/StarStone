using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Kurtis Watson
public class Clock_Controller : MonoBehaviour
{
    private Animator m_animator;

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

    public List<GameObject> clockParts = new List<GameObject>(); 

    private void Start()
    {
        portal.active = false;
        for (int i = 0; i < clockParts.Count; i++)
        {
            clockParts[i].active = false;
        }

        m_animator = GetComponentInChildren<Animator>();
        m_desiredMin = UnityEngine.Random.Range(1, 11);
        m_desiredHour = UnityEngine.Random.Range(1, 11);
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

        Debug.Log("Hour: " + m_desiredHour + " Min: " + m_desiredMin);
        if (Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                smallRotation += 30;
                m_currentHour += 1;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
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
            m_isActive = true;
            Debug.Log("Time is correct");
        }
        else
        {
            m_isActive = false;
        }
    }
}
