using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Wall : MonoBehaviour
{
    private Animator anim; //Reference animator component.

    void Start()
    {
        anim = GetComponent<Animator>(); //Access animator component.
        Invoke("f_lowerWall", 7); //Lower wall after 7 seconds.
    }

    void f_lowerWall()
    {
        anim.Play("anim_wallLower"); //Activate wall lower animation.
    }
}
