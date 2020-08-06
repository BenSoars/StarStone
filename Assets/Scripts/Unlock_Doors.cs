using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock_Doors : MonoBehaviour
{
    [Header("Door Components")]
    [Space(2)]
    public Animator anim; //Access animator.
    private Wave_System m_waveSystem; //Required script.

    private void Start()
    {
        m_waveSystem = FindObjectOfType<Wave_System>(); //Access required script.
    }
    void Update()
    {
        anim.SetBool("Open", m_waveSystem.m_startedWaves); //Open the doors.

        if(m_waveSystem.m_startedWaves == true) //Destroy object after 4 seconds (Invoked).
        {
            Invoke("f_removeObject", 4);
        }
    }

    void f_removeObject()
    {
        Destroy(gameObject); //Remove the game object.
    }
}
