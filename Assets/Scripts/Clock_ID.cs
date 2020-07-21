using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_ID : MonoBehaviour
{
    public int clockPartID;

    public bool pickedUp;
    public Light light;

    private float currentIntensity;

    private bool increaseIntensity;

    private Pickup_System pickupSystem;
    private void Start()
    {

    }

    private void Update()
    {
        if (light.intensity <= 0)
        {
            increaseIntensity = true;

        }
        else if (light.intensity >= 2)
        {
            increaseIntensity = false;
        }

        if (increaseIntensity == true)
        {
            currentIntensity += 0.03f;
        }
        else currentIntensity -= 0.03f;

        if (pickedUp == false)
        {
            light.enabled = true;
            light.intensity = currentIntensity;
        }
        else
        {
            light.enabled = false;
            light.intensity = 0;
        }
    }
}
