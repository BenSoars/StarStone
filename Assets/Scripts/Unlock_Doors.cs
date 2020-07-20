using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock_Doors : MonoBehaviour
{
    public Animator anim;
    // Update is called once per frame

    private Wave_System waveSystem;

    private void Start()
    {
        waveSystem = FindObjectOfType<Wave_System>();
    }
    void Update()
    {
        anim.SetBool("Open", waveSystem.m_startedWaves);

        if(waveSystem.m_startedWaves == true)
        {
            Invoke("f_removeObject", 4);
        }
    }

    void f_removeObject()
    {
        Destroy(gameObject);
    }
}
