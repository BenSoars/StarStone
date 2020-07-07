using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Portal : MonoBehaviour
{
    private Player_Controller r_playerController;

    public Transform m_desiredLocation;

    private void Start()
    {
        r_playerController = FindObjectOfType<Player_Controller>();       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            r_playerController.transform.position = m_desiredLocation.position;
        }
    }
}
