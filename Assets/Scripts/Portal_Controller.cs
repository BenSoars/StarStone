using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal_Controller : MonoBehaviour
{
    public Image transition;

    private float opacity;
    private bool transitionActive;

    private bool opacityMet;
    private void Start()
    {
        transition.enabled = false;
    }

    private void Update()
    {
        if(transitionActive == true)
        {
            if (opacityMet == false)
            {
                opacity += 0.001f;
                if(opacity >= 1)
                {
                    SceneManager.LoadScene("Temple_Clean");
                    opacityMet = true;
                }
            }

            if(opacityMet == true)
            {
                opacity -= 0.001f;
            }
            
            Color c = transition.color;
            c.a = opacity;
            transition.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            transitionActive = true;
            transition.enabled = true;
        }
    }
}
