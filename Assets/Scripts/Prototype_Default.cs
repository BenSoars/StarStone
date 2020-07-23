using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Default : MonoBehaviour
{
    public Transform shotPoint;
    public GameObject particles;
    private Animator anim;

    private bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(isPlaying == false)
            {
                isPlaying = true;
                Invoke("resetAnimation", 0.5f);
                anim.SetBool("Shake", isPlaying);
                Instantiate(particles, shotPoint);
            }                 
        }
    }

    void resetAnimation()
    {
        anim.SetBool("Shake", false);
        isPlaying = false;
    }
}
