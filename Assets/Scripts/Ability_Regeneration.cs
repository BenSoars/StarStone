using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Regeneration : MonoBehaviour
{
    [Header("Regeneration Values")]
    [Tooltip("How much health the player should regenerate per second when stood in the circle.")]
    public float healthPerSecond;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player_Controller>().playerHealth += 0.1f; //Increase player health when they are stood inside the trigger collider.
        }
    }
}
