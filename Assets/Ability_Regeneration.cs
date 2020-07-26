using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Regeneration : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player_Controller>().playerHealth += 0.1f; //Increase player health when they are stood inside the trigger collider.
        }
    }
}
