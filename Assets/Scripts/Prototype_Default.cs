using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Default : MonoBehaviour
{
    [Header("Default Staff Components")]
    [Space(2)]
    [Tooltip("Set position of particle point.")]
    public Transform shotPoint;
    [Tooltip("Particle component used for the 'spark'.")]
    public GameObject particles;
    private Animator m_anim; //Animator component reference. 
    private bool m_isPlaying; //Bool used to stop the player spamming the animation.

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>(); //Access the animator.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(m_isPlaying == false) //This loop stops the player from spamming the animation.
            {
                m_isPlaying = true;
                Invoke("resetAnimation", 0.5f);
                m_anim.SetBool("Shake", m_isPlaying); //Play animation.
                Instantiate(particles, shotPoint);
            }                 
        }
    }

    void resetAnimation()
    {
        m_anim.SetBool("Shake", false);
        m_isPlaying = false;
    }
}
