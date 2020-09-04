using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Starstone_ID : MonoBehaviour
{
    [Header("Referenced Scripts")]
    private Prototype_Classes m_prototypeClasses; //Reference prototype class to access Stone power.

    [Header("Starstone Components")]
    [Space(2)]
    [Tooltip("Preview image used for the 'Q' ability of this Starstone.")]
    public Sprite preview1; //Ability 'Q' preview on hover.
    [Tooltip("Preview image used for the 'V' ability of this Starstone.")]
    public Sprite preview2; //Ability 'V' preview on hover.
    [Tooltip("Set the ID of the stone this script is attached to.")]

    public string textPreview1;
    public string textPreview2;

    public int stoneID; //Stone ID to determine which stone the player is looking at.
    [Tooltip("Power bar displayed for the Starstone to visualise power left in the stone.")]
    public Image charge; //Used to visualise the amount of charge in the Starstone.

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
