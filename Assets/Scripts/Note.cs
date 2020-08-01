using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Note : MonoBehaviour
{
    // Ben Soars
    public string NoteName; // the Note name
    public string NoteText; // the Note's contents
    private Inventory m_invSystem; // the inventory system access

    //Kurtis Watson
    private User_Interface m_userInterface;

    void Start()
    {
        m_invSystem = GameObject.FindObjectOfType<Inventory>(); // get access to the note system
        m_userInterface = FindObjectOfType<User_Interface>();
    }

    // Start is called before the first frame update
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // if the player walks over the note
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Scene")) // if it's the tutorial level
            {
                Destroy(gameObject); // destroy self
            }
            else
            {
                pickUpNote(); // collect the note, run the function
            }
        }
    }

    void pickUpNote()
    {
        m_invSystem.noteName.Add(NoteName); // add to title the inventory list
        m_invSystem.noteContents.Add(NoteText); // add contents to the inventory list
        Destroy(gameObject); //destroy self

        //Kurtis Watson
        m_userInterface.collectedNotes += 1;
    }
}
