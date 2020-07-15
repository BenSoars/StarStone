using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Kurtis Watson
public class Portal_Controller : MonoBehaviour
{
    public GameObject transition;

    public GameObject player;

    private float opacity;
    private bool transitionActive;
    private bool sceneSwitch;

    private bool opacityMet;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(transitionActive == true) //Begin transition if the player sets off trigger.
        {
            if (opacityMet == false)
            {
                opacity += 0.002f;
                if(opacity >= 1)
                {
                    SceneManager.LoadScene("Temple_Clean"); //Load new scene as screen hits pitch black (transition effect).
                    opacityMet = true; //Begin lowering the opacity to show the new scene.
                }
            }

            if(opacityMet == true)
            {
                opacity -= 0.002f; //Lower the opacity as the new scene has loaded.  
                if(opacity <= 0) 
                {
                    transition.active = false; //Disable the transition image so that the UI buttons can be used.
                }
            }
            
            Color c = transition.GetComponent<Image>().color;
            c.a = opacity;
            transition.GetComponent<Image>().color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            transitionActive = true; //Activate transition.
            transition.active = true; //Enable GameObject.
        }
    }
}
