using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Wall : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("f_lowerWall", 7);
    }

    void f_lowerWall()
    {
        anim.Play("anim_wallLower");
    }
}
