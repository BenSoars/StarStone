using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Starstone_ID : MonoBehaviour
{
    [Header("Referenced Scripts")]
    private Prototype_Classes m_prototypeClasses;

    [Header("Starstone Components")]
    [Space(2)]
    public Sprite preview1;
    public Sprite preview2;
    public int stoneID;
    public Image charge;

    // Start is called before the first frame update
    void Start()
    {
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
    }

    // Update is called once per frame
    void Update()
    {
        charge.fillAmount = m_prototypeClasses.stonePower[stoneID] / 100; //This image is used for the the canvas that displays the starstone charge. 
    }
}
