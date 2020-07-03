using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    // Ben Soars
    public string NoteName; // the Note name
    public string NoteText; // the Note's contents
    public Sprite NoteImage; // an image for the note

    private Inventory m_invSystem; // the inventory system access

    void Start()
    {
        m_invSystem = GameObject.FindObjectOfType<Inventory>(); // get access to the note system
    }

    // Start is called before the first frame update
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // if the player walks over the note
        {
            pickUpNote(); // collect the note, run the function
        }
    }

    void pickUpNote()
    {
        m_invSystem.noteName.Add(NoteName); // add to the inventory list
        m_invSystem.noteContents.Add(NoteText); // add to the inventory list
        m_invSystem.noteImage.Add(NoteImage); // add to the inventory list
        Destroy(gameObject); // add to the inventory list
    }
}
