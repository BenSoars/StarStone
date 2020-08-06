using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Kurtis Watson
public class Portal_Controller : MonoBehaviour
{
    [Header("Portal Components")]
    [Tooltip("Sets the transition active so the scene switching is smoother.")]
    public bool transitionActive;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {           
            transitionActive = true; //Activate transition.
        }
    }
}
