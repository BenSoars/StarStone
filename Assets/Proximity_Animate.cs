using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity_Animate : MonoBehaviour
{
    // Ben Soars

    public Animator anim;
    public Player_Controller player;
    private float Distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player_Controller>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(player.transform.position, transform.position);
        anim.SetBool("Close", Distance <= 8);
    }
}
