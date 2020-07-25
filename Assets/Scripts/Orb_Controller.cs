using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Orb_Controller : MonoBehaviour
{
    [Header("Script References")]
    [Space(2)]
    private Prototype_Classes r_prototypeClasses;

    public List<Material> materials = new List<Material>();

    private void Start()
    {
        r_prototypeClasses = FindObjectOfType<Prototype_Classes>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().material = materials[r_prototypeClasses.chosenBuff]; //Change colour of orb based on chosen enemy buff.
    }
}
