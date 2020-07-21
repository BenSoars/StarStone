﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Rune_Controller : MonoBehaviour
{
    public GameObject wisp;
    public Animator animator;
    public Light light;
    public Renderer runeBall;
    public Renderer runeArt;
    public Material globalColor;

    private Prototype_Classes prototypeClasses;
    private Easter_Eggs easterEggs;

    private bool shake;
    public bool animated;

    public bool stopActivation;

    private void Start()
    {
        prototypeClasses = FindObjectOfType<Prototype_Classes>();
        easterEggs = FindObjectOfType<Easter_Eggs>();
    }
    private void Update()
    {
        animator.SetBool("Shake", shake);

        if (animated == true)
        {          
            shake = true;
            Invoke("resetShake", 5);
        }

        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = prototypeClasses.stoneColor;
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = prototypeClasses.stoneColor;
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = prototypeClasses.stoneColor;
        wisp.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = prototypeClasses.stoneColor;
        light.color = prototypeClasses.stoneColor;
        runeBall.material.color = prototypeClasses.stoneColor;
        runeArt.material.SetColor("_EmissionColor", prototypeClasses.stoneColor);
        globalColor.SetColor("_EmissionColor", prototypeClasses.stoneColor);


    }

    public void f_runeEasterEgg()
    {
        if(stopActivation == false)
        {
            stopActivation = true;
            easterEggs.runesFound += 1;
        }
    }

    void resetShake()
    {
        animated = false;
        shake = false;
    }
}
