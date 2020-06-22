﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    // Ben Soars
    private NavMeshAgent m_navAgent;
    public float m_moveSpeed = 2;
    public float m_runSpeed = 4;
    public float m_enemyHealth = 3;
    public int m_spawnChance = 3;
    public bool m_isStunned;

    private Player_Controller r_player;
    private Wave_System r_waveSystem;
    private Rigidbody m_rb;

    public GameObject m_AmmoCrate;
    public bool m_isGrounded = true;

    public Vector3 m_lastPosition;
    private int m_randomNumber;

    public float m_enemyDamage;

    [System.Serializable]
    public enum CurrentState // The current state
    {
        Wander,
        Attack,
        Check
    }
    public CurrentState m_state;

    public Transform m_eyePos;
    private RaycastHit m_sightRaycast; // the hitscan raycast

    //Kurtis Watson
    public GameObject m_whisp;
    public bool m_isEnemyStunned;

    private float m_defaultRunSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>(); // get rigidbody
        m_navAgent = gameObject.GetComponent<NavMeshAgent>();

        r_player = GameObject.FindObjectOfType<Player_Controller>();
        r_waveSystem = FindObjectOfType<Wave_System>();

        m_defaultRunSpeed = m_runSpeed;
    }

    void Update()
    {
        //Kurtis Watson
        if (m_isEnemyStunned == true)
        {
            m_isEnemyStunned = false;
            Invoke("f_resetSpeed", 5);
        }

        // line of sight
        Vector3 newDirection = (r_player.transform.localPosition - m_eyePos.position).normalized;
        m_eyePos.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, newDirection.y, newDirection.z));

        if (Physics.Raycast(m_eyePos.position, m_eyePos.forward, out m_sightRaycast, Mathf.Infinity)) // shoot out a raycast for hitscan
        {
            Debug.DrawRay(m_eyePos.position, m_eyePos.forward * m_sightRaycast.distance, Color.yellow); // draw line only viewable ineditor
            if (m_sightRaycast.collider.gameObject.CompareTag("Player") && r_player.m_isPlayerInvisible == false)
            {
                m_state = CurrentState.Attack;
                m_lastPosition = r_player.transform.position;

            }
            else if (m_state == CurrentState.Attack)
            {
                m_state = CurrentState.Check;
            }

            //TESTING PURPOSES
            if (Vector3.Distance(transform.position, r_player.transform.position) <= 1)
            {
                r_player.m_playerHealth -= m_enemyDamage * r_player.m_defenceValue;
            }

            //Ben Soars
            if (m_enemyHealth <= 0)
            {
                int rando = UnityEngine.Random.Range(0, m_spawnChance);
                if (rando == 1)
                {
                    Instantiate(m_AmmoCrate, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);

                //Kurtis Watson
                GameObject isNewWisp = Instantiate(m_whisp, transform.position, Quaternion.identity);
                isNewWisp.GetComponent<Wisp_Controller>().m_enemySpawn = true;
                r_waveSystem.enemiesLeft -= 1;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_navAgent.enabled = m_isGrounded;
        if (m_isGrounded == true && m_isStunned == false) // movement
        {
            m_rb.velocity = Vector3.zero;
            switch (m_state) // the current enemy state
            {
                case (CurrentState.Attack): // if they're set to attack 
                        m_navAgent.SetDestination(r_player.transform.position);
                        m_navAgent.speed = m_runSpeed; // set to defined movespeed in script for consistancy's sake
                    break;
                case (CurrentState.Check): // if enemy has lost player, search for last known position
                        m_navAgent.SetDestination(m_lastPosition);

                        m_navAgent.speed = m_runSpeed; // set to defined movespeed in script for consistancy's sake
                                                       //Debug.Log("Searching Last Known Position");
                        if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                        {
                            //Debug.Log("Lost Player");
                            m_state = CurrentState.Wander;
                        }
                    break;
                default: // if wandering
                    m_navAgent.SetDestination(r_waveSystem.spawnPoints[m_randomNumber].position);

                    if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                    {
                        m_randomNumber = UnityEngine.Random.Range(0, r_waveSystem.spawnPoints.Count);
                    }
                    //Debug.Log("Looking for Player");
                    m_navAgent.speed = m_moveSpeed; // set to defined movespeed in script for consistancy's sake
                    break;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            m_isGrounded = false;
            m_enemyHealth = 1;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && m_isGrounded == false)
        {
            m_enemyHealth = 0;
        }
    }

    //Kurtis Watson
    void f_resetSpeed()
    {
        m_runSpeed = m_defaultRunSpeed;
    }
}
