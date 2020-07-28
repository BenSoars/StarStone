using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Pushback : MonoBehaviour
{
    [Header("Pushback Forces")]
    [Space(2)]

    [Tooltip("Set the explosion force (how much the player is pushed by the force).")]
    public float explosionForce;
    [Tooltip("Set how high the player is lifted when inside the trigger.")]
    public float upwardsForce;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            Rigidbody player = other.gameObject.GetComponent<Rigidbody>(); //Find the players rigidbody.
            player.AddExplosionForce(explosionForce, transform.position, 200); //Apply an explosion force to the rigidbody.
            player.AddForce(transform.up * upwardsForce); //Add an uplift force to the rigidbody.
            player.velocity = Vector3.zero; //Set the players velocity to 0;
        }
    }
}
