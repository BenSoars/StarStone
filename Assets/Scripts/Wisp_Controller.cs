using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wisp_Controller : MonoBehaviour
{
    //Kurtis Watson
    private NavMeshAgent m_navAgent;
    private Player_Controller r_player;

    private GameObject[] wispPoint;

    private int m_random;

    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = gameObject.GetComponent<NavMeshAgent>();
        r_player = GameObject.FindObjectOfType<Player_Controller>();

        wispPoint = GameObject.FindGameObjectsWithTag("WispPoint");

        m_random = Random.Range(0, 4);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_navAgent.SetDestination(wispPoint[m_random].transform.position);

        if(this.transform.position.x <= m_navAgent.destination.x + 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
