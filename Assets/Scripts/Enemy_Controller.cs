﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    // Ben Soars
    private NavMeshAgent m_navAgent;

    [Tooltip("Enemy movement speed, used when the enemy is wandering around")]
    public float m_moveSpeed = 2; // the default movement speed
    [Tooltip("Enemy running speed, used when the enemy spots the player")]
    public float m_runSpeed = 4; // the running speed, used when it spots the player
    public float m_enemyHealth = 3; // the enemy health
    public float m_enemyDamage = 5; // the damage the enemy does to the player
    public float m_attackTime = 2; //the time inbetween attacks
    public int m_spawnChance = 3; // chance for it to spawn an item on death
    public bool m_isStunned; // is stunend
    private bool m_resetStun; // reset the stun
    private bool m_isAttacking; // used to tell if the enemy is currently attacking

    public bool m_isRanged = false; // if the enemy is a ranged type
    public Projectile m_projectile; // the projectile they fire

    // access to other componenets
    private Player_Controller r_player; 
    private Wave_System r_waveSystem;
    private Rigidbody m_rb;
    private Animator r_anim;

    public List<GameObject> m_ItemDrops = new List<GameObject>(); // item drop
    public bool m_isGrounded = true; // is grounded check

    private Vector3 m_lastPosition; // the last position the enemy saw the player at
    private int m_randomNumber;

    [System.Serializable]
    public enum CurrentState // The current state
    {
        Wander,
        Attack,
        Check
    }
    public CurrentState m_state; // the current state

    public Transform m_eyePos; // the sight light position
    private RaycastHit m_sightRaycast; // the hitscan raycast
    public LayerMask layerMask; // the gameobject layer which sightlines will ignore

    public Enemy_Damage m_hurtBox; // the enemy hurtbox

    public AudioSource soundEffect;
    public List<AudioClip> soundsList = new List<AudioClip>(); 

    //Kurtis Watson
    private Prototype_Classes r_prototypeClasses; 

    public GameObject m_whisp;
    public bool m_isEnemyInfected;
    private bool m_previouslyInfected;
    private bool m_particleSystem;

    private float m_defaultRunSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // get the components that are defined at the top
        m_rb = gameObject.GetComponent<Rigidbody>(); // get rigidbody
        m_navAgent = gameObject.GetComponent<NavMeshAgent>(); 
        r_anim = gameObject.GetComponent<Animator>();
        r_player = GameObject.FindObjectOfType<Player_Controller>();
        r_waveSystem = FindObjectOfType<Wave_System>();
        r_prototypeClasses = FindObjectOfType<Prototype_Classes>();

        m_defaultRunSpeed = m_runSpeed; // set the default run speed
        m_hurtBox.m_damage = m_enemyDamage; // set the hurtbox damage to represent the enemy damage

        soundEffect.volume = PlayerPrefs.GetFloat("volumeLevel"); // set the sound to match the sound effect volume
    }

    void Update()
    {
        //Kurtis Watson
        if (m_isEnemyInfected == true && m_previouslyInfected == false) 
        {
            if (m_particleSystem == false)
            {
                m_particleSystem = true;
                GetComponentInChildren<ParticleSystem>().Play();
            }

            m_enemyHealth -= 0.1f;
            GetComponentInChildren<BoxCollider>().enabled = true;
            Invoke("f_resetInfection", 10);
        }       

        //Ben Soars 

        // line of sight
        Vector3 newDirection = (r_player.transform.localPosition - m_eyePos.position).normalized; // look at the player
        m_eyePos.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, newDirection.y, newDirection.z)); // looking at the player

        // resetting stunned state
        if (m_isStunned == true && m_resetStun == false)
        {
            m_resetStun = true; // reset the stun
            Invoke("f_resetStun", 2); // reset the stun after 2 seconds
        }

        // if the enemy is dead
        if (m_enemyHealth <= 0)
        {
            //Kurtis Watson
            r_prototypeClasses.m_currentFog = r_prototypeClasses.m_currentFog - r_waveSystem.m_fogMath; 

            //Ben Soars
            int rando = UnityEngine.Random.Range(0, m_spawnChance); // generate a random nomber whtihin the range
            if (rando == 1) 
            {
                rando = UnityEngine.Random.Range(0, m_ItemDrops.Count); // choose which item to spawn
                Instantiate(m_ItemDrops[rando], transform.position, Quaternion.identity); //spawn the item it chose
            }

            Destroy(gameObject); // destroy self

            //Kurtis Watson
            GameObject isNewWisp = Instantiate(m_whisp, transform.position, Quaternion.identity); // spawn a wisp
            isNewWisp.GetComponent<Wisp_Controller>().m_enemySpawn = true; // set the wisp to recognise it was spawned from a defeated enemy
            r_waveSystem.enemiesLeft -= 1; // take away from the enemies left
        }



        if (Physics.Raycast(m_eyePos.position, m_eyePos.forward, out m_sightRaycast, Mathf.Infinity, layerMask)) // shoot out a raycast for hitscan
        {
            Debug.DrawRay(m_eyePos.position, m_eyePos.forward * m_sightRaycast.distance, Color.yellow); // draw line only viewable ineditor
            if (m_sightRaycast.collider.gameObject.CompareTag("Player") && r_player.isPlayerInvisible == false) // if it can see the player 
            {
                m_state = CurrentState.Attack; // set the enemy to be attacking
                m_lastPosition = r_player.transform.position; // set the last position of the player

            }
            else if (m_state == CurrentState.Attack) // if they're attacking and they can't see the player
            {
                m_state = CurrentState.Check; // set to check for the player
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

                    if (m_isRanged == true) // if the enemy is ranged
                    {
                        if (m_isAttacking == false)
                        {
                            StartCoroutine("CanAttack"); // begin attack
                        }
                    }
                    else
                    {
                        if (m_isAttacking == false)
                        {
                            m_navAgent.SetDestination(r_player.transform.position); // head towards the player
                            m_navAgent.speed = m_runSpeed; // set to defined movespeed in script for consistancy's sake
                            if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance) // if the enemy is close enough to attack
                            {
                                StartCoroutine("CanAttack"); // begin attack
                            }
                        }
                    }
                    break;
                case (CurrentState.Check): // if enemy has lost player, search for last known position
                        m_navAgent.SetDestination(m_lastPosition); 

                        m_navAgent.speed = m_runSpeed; // set to defined movespeed in script for consistancy's sake
                  
                        if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance) // if the enemy arrives at the last known location and there is no sign of the player
                        {
                            m_state = CurrentState.Wander; // set back to wander
                        }
                    break;
                default: // if wandering
                    m_navAgent.SetDestination(r_waveSystem.spawnPoints[m_randomNumber].position); // set the destination for the enemy based on the random number generated

                    if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)  // if the enemy is at their destination, within their stop distance range
                    {
                        m_randomNumber = UnityEngine.Random.Range(0, r_waveSystem.spawnPoints.Count); // generate a new destination using the list of destinations on the wave system
                    }
                    
                    m_navAgent.speed = m_moveSpeed; // set to defined movespeed in script for consistancy's sake
                    break;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion")) // if the enemy is hit by an explosion
        {
            m_isGrounded = false; // set them to be no longer grounded
            m_enemyHealth = 1; // set them to 1 health so they die on impact
        }

        if (other.gameObject.CompareTag("Infected")) // if the enemy touches an infection point
        {
            m_isEnemyInfected = true; // set themselves to be infected
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Melee")) // if the enemy is hit by an explosion
        {
            
            m_enemyHealth -= 50; // set them to 1 health so they die on impact
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && m_isGrounded == false) // if the enemy touches the ground and isn't already grounded
        {
            m_enemyHealth = 0; // set the enemy to die on impact
        }

        
    }

    IEnumerator CanAttack() // attack function
    {
        if (m_isAttacking == false)
        {
            m_isAttacking = true; // set the enemmy to be attacking to prevent this coroutine from overlapping itself
            yield return new WaitForSeconds(0.1f); // wait a short time so it's not instant
            if (!m_isRanged) // if they are not a ranged type
            {
                r_anim.SetTrigger("Attack"); // play attacking animation
            } else
            {
                Projectile proj = Instantiate(m_projectile, m_eyePos.position, Quaternion.identity); // create projectile at shot point
                proj.GetComponent<Rigidbody>().AddForce(m_eyePos.forward * 200); // push the projectile forwards with rigidbody
                proj.m_enemy = true; // set the projectile to be an enemy projectile
                proj.m_damage = m_enemyDamage; // set the damage of the projectile to reflect the enemy damage
            }
            soundEffect.clip = soundsList[0]; // set the sound effect to be the attack sound
            soundEffect.Play(); // play the attack sound
            yield return new WaitForSeconds(m_attackTime); // wait until they can attack again
            m_isAttacking = false;
        }
       
    }

    void f_resetStun() // reset the enemy stun state
    {
        m_isStunned = false;
        m_resetStun = false;
    }

    //Kurtis Watson
    void f_resetSpeed()
    {
        m_runSpeed = m_defaultRunSpeed; // set run speed to default
    }

    //Kurtis Watson
    void f_resetInfection()
    {
        // reset the infection 
        m_previouslyInfected = true;
        m_isEnemyInfected = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponentInChildren<BoxCollider>().enabled = false;       
    }
}
