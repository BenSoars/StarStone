using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Rune_Controller : MonoBehaviour
{   
    [Header("Referenced Scripts")]
    private Prototype_Classes m_prototypeClasses; //References the required scripts.
    private Easter_Eggs m_easterEggs;

    [Header("Rune Components")]
    [Space(2)]   
    [Tooltip("Attach animator of the Rune.")]
    public Animator animator;
    [Tooltip("Light object that changes colour based on enemy buff.")]
    public Light light;
    [Tooltip("Ball object that changes colour based on enemy buff.")]
    public Renderer runeBall;
    [Tooltip("Rune stripes that changes colour based on enemy buff.")]
    public Renderer runeArt;
    [Tooltip("Global colour that all objects will be set to based on enemy buff.")]
    public Material globalColor; //Colour that will be used by several objects.
    public bool stopActivation;
    private bool m_shake; //Bool that is used to set the shake animation true/false.
    [Tooltip("Check to see if the Runes are currently animating.")]
    public bool animated;
 
    [Header("Wisp Components")]
    [Space(2)]
    [Tooltip("Wisp game object.")]
    public GameObject wisp;

    private void Start()
    {
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        m_easterEggs = FindObjectOfType<Easter_Eggs>();
    }
    private void Update()
    {
        animator.SetBool("Shake", m_shake);

        if (animated == true)
        {          
            m_shake = true; //Start shaking runes animation.
            Invoke("reset_shake", 5); 
        }

        f_setGlobalColours();
    }

    private void f_setGlobalColours()
    {
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = m_prototypeClasses.stoneColor; //Access the wisp game object and change the values of the particles.
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = m_prototypeClasses.stoneColor;
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = m_prototypeClasses.stoneColor;
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = m_prototypeClasses.stoneColor;
        light.color = m_prototypeClasses.stoneColor; //Change the light of the run based on stone color.
        runeBall.material.color = m_prototypeClasses.stoneColor; //Change the colour of the rune ball based on stone color.
        runeArt.material.SetColor("_EmissionColor", m_prototypeClasses.stoneColor); //Change emission colour.
        globalColor.SetColor("_EmissionColor", m_prototypeClasses.stoneColor);
    }

    public void f_runeEasterEgg()
    {
        if(stopActivation == false) //Stop the runes found being increased more than 1.
        {
            stopActivation = true;
            m_easterEggs.runesFound += 1; //Add to the runes found value; this is to allow for an easter egg to happen in game.
        }
    }

    void reset_shake()
    {
        animated = false;
        m_shake = false; //End shaking runes animation.
    }
}
