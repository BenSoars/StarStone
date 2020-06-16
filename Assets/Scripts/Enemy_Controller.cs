using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    // Ben Soars
    private NavMeshAgent m_navAgent;
    public float m_moveSpeed = 4;
    public float m_enemyHealth = 3;
    public int m_spawnChance = 3;

    //Kurtis Watson
    public float m_detectionRadius;
    public bool m_isDetected;
    public GameObject m_whisp;

    //Ben Soars
    private Player_Controller r_player;
    private Wave_System r_waveSystem;
    private Rigidbody m_rb;

    public GameObject m_AmmoCrate;
    public bool m_isGrounded = true;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>(); // get rigidbody
        m_navAgent = gameObject.GetComponent<NavMeshAgent>();
        m_navAgent.speed = m_moveSpeed; // set to defined movespeed in script for consistancy's sake
        r_player = GameObject.FindObjectOfType<Player_Controller>();
        r_waveSystem = FindObjectOfType<Wave_System>();
    }

    void Update()
    {
        //Kurtis Watson
        if (Vector3.Distance(r_player.transform.position, this.transform.position) < m_detectionRadius)
        {
            m_isDetected = true; //Used below in if statement to check if the enemy is within range.
        }
        // TO DO: Give enemy sightline (using Raycast), if player enters they chase, if they exit they travel to last seen point and continue looking. If they can't find the player they stand still

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

    // Update is called once per frame
    void FixedUpdate()
        {
            m_navAgent.enabled = m_isGrounded;
            if (m_isGrounded == true && m_isDetected == true)
            {
                m_rb.velocity = Vector3.zero;
                m_navAgent.SetDestination(r_player.transform.position);
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
}
