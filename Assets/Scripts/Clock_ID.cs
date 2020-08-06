using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Clock_ID : MonoBehaviour
{
    [Header("Clock Mechanics")]
    [Space(2)]
    [Tooltip("Set the ID of this clock part.")]
    public int clockPartID; //Clock part ID.
    [Tooltip("Checks if the item is picked up.")]
    public bool pickedUp; //Is the item picked up bool.


    [Header("Light Mechanics")]
    [Space(2)]
    [Tooltip("Reference the light component of this game object.")]
    public Light light;
    private float m_currentIntensity; //Current light intensity.
    private bool m_increaseIntensity; //Increase intensity.

    private void Update()
    {
        f_lightEffect();
    }

    void f_lightEffect()
    {
        if (light.intensity <= 0) //Check the intensity, if it below 0 then increase the intensity.
        {
            m_increaseIntensity = true;

        }
        else if (light.intensity >= 2) //If the intensity is above 2 then lower the intensity.
        {
            m_increaseIntensity = false;
        }

        if (m_increaseIntensity == true)
        {
            m_currentIntensity += 0.03f; //Increase intensity.
        }
        else m_currentIntensity -= 0.03f; //Decrease intensity.


        if (pickedUp == false)
        {
            light.enabled = true; //Enable light if the player isn't holding it.
            light.intensity = m_currentIntensity;
        }
        else
        {
            light.enabled = false;
            light.intensity = 0; //Reset light intensity.
        }
    }
}
