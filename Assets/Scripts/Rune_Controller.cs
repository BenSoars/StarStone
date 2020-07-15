using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Rune_Controller : MonoBehaviour
{
    public GameObject wisp;
    private Animator animator;
    public Light light;
    public Renderer runeBall;
    public Renderer runeArt;
    public Material globalColor;

    private Prototype_Classes prototypeClasses;

    private bool shake;
    public bool animated;

    private void Start()
    {
        animator = GetComponent<Animator>();
        prototypeClasses = FindObjectOfType<Prototype_Classes>();
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

    void resetShake()
    {
        animated = false;
        shake = false;
    }
}
