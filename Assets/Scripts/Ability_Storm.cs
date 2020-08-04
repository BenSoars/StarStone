using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Ability_Storm : MonoBehaviour
{
    [Header("Storm Values")]
    [Tooltip("Set the damage per second for the storm.")]
    public float stormDamage = 2;
    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false; //Disable trigger until cloud has risen.
        GetComponent<ParticleSystem>().Stop();
        Invoke("f_startRain", 2); //Animation of cloud rising is 2 seconds long as so when the cloud is in the correct position, the rain will begin.
    } 
    
    void f_startRain() //Start the particle effect.
    {
        GetComponent<BoxCollider>().enabled = true; //Enable trigger to start damaging enemies.
        GetComponent<ParticleSystem>().Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) //Check for enemy collision.
        {
            GameObject enemyHit = other.gameObject; //Access the specific collided object.
            enemyHit.GetComponent<Enemy_Controller>().m_enemyHealth -= stormDamage * Time.deltaTime; //Damage player -2 each hit.
        }
    }

   
}
