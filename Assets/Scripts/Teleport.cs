using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Teleport : MonoBehaviour
{
    [Header("Teleport Components")]
    [Space(2)]
    private Player_Controller m_playerController;
    public Transform desiredLocation;

    private void Start()
    {
        m_playerController = FindObjectOfType<Player_Controller>();       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && FindObjectOfType<Wave_System>().m_startedWaves == true)
        {
            m_playerController.transform.position = desiredLocation.position; //Change player position to position of desired location.
        }
    }
}
