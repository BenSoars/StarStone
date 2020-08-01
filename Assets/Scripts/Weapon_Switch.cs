using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Switch : MonoBehaviour
{
    // Ben Soars
    public string keyPressed; // the key that's been pressed 
    private string m_prevPressed; // store the previously pressed button
    public List<GameObject> m_Weapons = new List<GameObject>(); // the weapon gameobjects

    public int currentWeapon;

    //Kurtis Watson
    private Prototype_Classes prototypeClasses;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = 1; // set default weapon
        prototypeClasses = FindObjectOfType<Prototype_Classes>();
    }

    // Update is called once per frame
    void Update()
    {
        keyPressed = Input.inputString; // get the input button of the player
        if (int.TryParse(keyPressed, out int number)) // if the pressed button can be turned into an int
        {
            if (number <= m_Weapons.Count) // if the number is smaller or the same as the amount of weapons
            {
                currentWeapon = number; // set the current weapon
                
            }
        }

        if (prototypeClasses.canSwitch == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                currentWeapon++; // increase current weapon number
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                currentWeapon--; // decrease current weapon number
            }

            if (currentWeapon > m_Weapons.Count) // if the current weapon is larger than the total count
            {
                currentWeapon = 1; // set to 1, to reflect the first weapon
            }
            else if (currentWeapon < 1)
            {
                currentWeapon = m_Weapons.Count; // set to the total amount to reflect the last in the array
            }
        }

        disableAll(); // check the weapon active status

    }

    void disableAll()
    {
        for (int i = 0; i < m_Weapons.Count; i++)
        {
            if (i == currentWeapon - 1) {  // if it's the active weapon, take a way 1 from the number as the list starts at 0, but they keyboard input starts at 1
                m_Weapons[i].active = true; // set weapon active
            } else
            {
                m_Weapons[i].active = false; // disable weapon
            }
        }
    }
    
}
