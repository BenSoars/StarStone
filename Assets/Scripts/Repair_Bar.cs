using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Kurtis Watson
public class Repair_Bar : MonoBehaviour
{
    [Header("Repair Bar Components")]
    [Space(2)]
    private Pickup_System m_pickupSystem;
    public Image loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        m_pickupSystem = FindObjectOfType<Pickup_System>(); //Access pickup script.
    }

    // Update is called once per frame
    void Update()
    {
        loadingBar.fillAmount = m_pickupSystem.currentRepairTime / 5; //Sets the fill amount of a 'time left to repair' bar so the player can visualise repair time.
    }
}
