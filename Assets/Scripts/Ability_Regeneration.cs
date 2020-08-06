using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Regeneration : MonoBehaviour
{
    [Header("Regeneration Values")]
    [Tooltip("How much health the player should regenerate per second when stood in the circle.")]
    public float healthPerSecond; //Set regen value.
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player_Controller>().playerHealth += healthPerSecond * Time.deltaTime; //Increase player health when they are stood inside the trigger collider.
        }
    }
}
