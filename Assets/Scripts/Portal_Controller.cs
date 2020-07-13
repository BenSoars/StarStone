using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal_Controller : MonoBehaviour
{
    public Image transition;

    public GameObject player;

    private float opacity;
    private bool transitionActive;
    private bool sceneSwitch;

    private bool opacityMet;
    private void Start()
    {
        transition.enabled = false;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Temple_Clean" && sceneSwitch == false)
        {
            opacity = 1;
            Debug.Log("yes");
            sceneSwitch = true;
        }

        if(transitionActive == true)
        {
            if (opacityMet == false)
            {
                opacity += 0.002f;
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
