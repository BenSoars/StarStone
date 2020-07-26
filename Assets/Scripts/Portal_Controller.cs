using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Kurtis Watson
public class Portal_Controller : MonoBehaviour
{
    public bool transitionActive;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {           
            transitionActive = true; //Activate transition.
        }
    }
}
