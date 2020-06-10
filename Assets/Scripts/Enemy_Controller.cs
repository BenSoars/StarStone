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

    private Player_Controller m_player;

    public GameObject m_AmmoCrate;

    // Start is called before the first frame update
    void Start()
    {
        
        m_navAgent = gameObject.GetComponent<NavMeshAgent>();
        m_navAgent.speed = m_moveSpeed; // set to defined movespeed in script for consistancy's sake
        m_player = GameObject.FindObjectOfType<Player_Controller>();
    }

    void Update()
    {
        if (m_enemyHealth <= 0)
        {
            int rando = Random.Range(0, m_spawnChance);
            if (rando == 1)
            {
                Instantiate(m_AmmoCrate, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_navAgent.SetDestination(m_player.transform.position);
    }
}
