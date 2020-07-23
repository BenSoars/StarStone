using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Starstone_ID : MonoBehaviour
{
    public Sprite preview1;
    public Sprite preview2;

    private Prototype_Classes prototypeClasses;

    public int stoneID;
    public Image charge;
    // Start is called before the first frame update
    void Start()
    {
        prototypeClasses = FindObjectOfType<Prototype_Classes>();
    }

    // Update is called once per frame
    void Update()
    {
        charge.fillAmount = prototypeClasses.stonePower[stoneID] / 100;
    }
}
