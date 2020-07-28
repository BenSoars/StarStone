using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Text : MonoBehaviour
{
    [Header("Text to Show")]
    [Tooltip("The text that is shown when the player is looking at the object.")]
    public string text; //Any object with this attached to can have it's own customised text when the player is looking at an object close enough ('F' to Pickup).

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
