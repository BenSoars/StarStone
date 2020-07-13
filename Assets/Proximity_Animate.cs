using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity_Animate : MonoBehaviour
{
    // Ben Soars

    public Animator anim; // animator
    public Player_Controller player; // the player
    private float Distance;

    // Start is called before the first frame update
    void Start()
    {
        // get neccesary componenets
        player = GameObject.FindObjectOfType<Player_Controller>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the distance
        Distance = Vector3.Distance(player.transform.position, transform.position);
        anim.SetBool("Close", Distance <= 8); // if the player is within this range, set the animation to be true
    }
}
