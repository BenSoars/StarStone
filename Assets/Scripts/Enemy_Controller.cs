using System;
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

    private Player_Controller r_player;
    private Wave_System r_waveSystem;
    private Rigidbody m_rb;

    public GameObject m_AmmoCrate;
    public bool m_isGrounded = true;

    public Vector3 m_lastPosition;
    private int m_randomNumber;

    [System.Serializable]
    public enum CurrentState // The current state
    {
        Wander,
        Attack,
        Check
    }
    public CurrentState m_state;

    private Transform m_eyePos;
    private RaycastHit m_sightRaycast; // the hitscan raycast
  
 
    //Kurtis Watson
    public float m_detectionRadius;
    public bool m_isDetected;
    public GameObject m_whisp;

   

    // Start is called before the first frame update
    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>(); // get rigidbody
        m_navAgent = gameObject.GetComponent<NavMeshAgent>();
        
        r_player = GameObject.FindObjectOfType<Player_Controller>();
        r_waveSystem = FindObjectOfType<Wave_System>();
        m_eyePos = gameObject.GetComponentInChildren<Transform>();
    }

    void Update()
    {
        // line of sight
        Vector3 newDirection = (r_player.transform.position - transform.position).normalized;
        m_eyePos.rotation = Quaternion.LookRotation(newDirection);

        if (Physics.Raycast(m_eyePos.position, m_eyePos.forward, out m_sightRaycast, Mathf.Infinity)) // shoot out a raycast for hitscan
        {
            if (m_sightRaycast.collider.gameObject.CompareTag("Player"))
            {
                m_state = CurrentState.Attack;
                m_lastPosition = r_player.transform.position;

            } else if (m_state == CurrentState.Attack)
            {
                m_state = CurrentState.Check;
            }
        }

            //Kurtis Watson
            //if (Vector3.Distance(r_player.transform.position, this.transform.position) < m_detectionRadius)
            //{
            //    m_isDetected = true; //Used below in if statement to check if the enemy is within range.
            //}
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


        if (m_isGrounded == true) // movement
        {
            m_rb.velocity = Vector3.zero;
            switch (m_state) // the current enemy state
            {
                case (CurrentState.Attack): // if they're set to attack
                    
                    m_navAgent.SetDestination(r_player.transform.position);
                    m_navAgent.speed = m_runSpeed; // set to defined movespeed in script for consistancy's sake
                    Debug.Log("Found Player");
                    break;
                case (CurrentState.Check): // if enemy has lost player, search for last known position
                    m_navAgent.SetDestination(m_lastPosition);

                    m_navAgent.speed = m_runSpeed; // set to defined movespeed in script for consistancy's sake
                    Debug.Log("Searching Last Known Position");
                    if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                    {
                        Debug.Log("Lost Player");
                        m_state = CurrentState.Wander;
                    }
                    break;
                default: // if wandering

                    m_navAgent.SetDestination(r_waveSystem.spawnPoints[m_randomNumber].position);

                    if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                    {
                        m_randomNumber = UnityEngine.Random.Range(0, r_waveSystem.spawnPoints.Count);
                    }
                    Debug.Log("Looking for Player");
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
}
