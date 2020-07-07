using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Ability_Storm : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<ParticleSystem>().Stop();
        Invoke("f_startRain", 2); //Animation of cloud rising is 2 seconds long as so when the cloud is in the correct position, the rain will begin.
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) //Check for enemy collision.
        {
            GameObject enemyHit = other.gameObject; //Access the specific collided object.
            enemyHit.GetComponent<Enemy_Controller>().m_enemyHealth -= 2f; //Damage player -2 each hit.
        }
    }

    void f_startRain()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<ParticleSystem>().Play();
    }
}
