using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    // Ben Soars

    public float activeTIme = 2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("f_Delete", activeTIme);
    }

    void f_Delete()
    {
        Destroy(gameObject);
    }
}
