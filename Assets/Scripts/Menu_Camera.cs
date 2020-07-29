using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Camera : MonoBehaviour
{
    public Transform desiredLocation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
