using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    // Ben Soars

    public bool m_ExplodeOnContact; // if the object is to explode on contact with the floor
    public float m_Timer; // the time for the obecjt to explode

    public GameObject Explosion;

    // Update is called once per frame
    void Update()
    {
        Invoke("f_createExplosion", m_Timer); // invoke using the time passed in
    }

    void OnCollisionEnter(Collision other) 
    {
        if (m_ExplodeOnContact) // if it can explode on contact and hits something
        { 
            f_createExplosion(); // explode
        }
    }

    void f_createExplosion() // create the explosion
    {
        Instantiate(Explosion, transform.position, Quaternion.identity); // instanciate the explosion effect
        Destroy(this.gameObject); // destroy this
    }
}
