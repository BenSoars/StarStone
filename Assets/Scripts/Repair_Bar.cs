using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Kurtis Watson
public class Repair_Bar : MonoBehaviour
{
    private Pickup_System pickupSystem;

    public Image loadingBar;

    private float maxTotal = 1;
    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<Pickup_System>();
    }

    // Update is called once per frame
    void Update()
    {
        loadingBar.fillAmount = pickupSystem.currentRepairTime / (maxTotal * 5);
    }
}
