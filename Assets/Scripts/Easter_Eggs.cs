using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kurtis Watson
public class Easter_Eggs : MonoBehaviour
{
    [Header("Raycast Components")]
    [Space(2)]
    [Tooltip("Camera object attached to player.")]
    public Transform camera; //Player camera.

    [Header("Rune Components")]
    [Space(2)]
    [Tooltip("Total amount of runes found.")]
    public int runesFound; //Runes found counter.
    private AchievementTracker m_Achievement; //Achievement tracker reference.

    [Header("Audio Components")]
    [Space(2)]
    [Tooltip("The audio source for the easter egg music.")]
    public AudioSource audioSource;
    [Tooltip("The song to play for the easter egg.")]
    public AudioClip song;
    private bool m_musicPlayed; //Stop the song looping.

    // Start is called before the first frame update
    void Start()
    {
        m_Achievement = GameObject.FindObjectOfType<AchievementTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        f_runesEasterEgg();
    }

    void f_runesEasterEgg()
    {
        RaycastHit m_objectHit;

        if (Physics.Raycast(camera.position, camera.forward, out m_objectHit, 3f)) //Create a raycast.
        {
            float distance = Vector3.Distance(camera.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.name == "Rune") && distance <= 3 && Input.GetKeyDown("f")) //Check if player is looking at a rune.
            {
                m_objectHit.collider.gameObject.GetComponentInChildren<Rune_Controller>().f_runeEasterEgg();
            }
        }

        if(runesFound == 5 && m_musicPlayed == false)
        {
            m_Achievement.UnlockAchievement(11); // unlock the Achievement
            m_musicPlayed = true;
            audioSource.clip = song; //Set the song.
            audioSource.Play(); //Play the song.
        }
    }
}
