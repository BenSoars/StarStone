using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Kurtis Watson
public class Cutscene_Handler : MonoBehaviour
{
    [Header("Text Components")]
    [Space(2)]
    [Tooltip("Text to display during the cutscenes. Number '5' is the cut off point for first cutscene.")]
    public TextMeshProUGUI cutsceneTextMesh;
    [Tooltip("Set the text values for the different notes.")]
    public string[] cutsceneText; //String array for notes.
    private int m_index; //Index value for current text display.
    [Tooltip("How fast each letter should appear one after the other.")]
    public float typewriterSpeed; //Speed at which the character appear.
    private bool m_stopTextLoop;

    [Header("Cutscene Components")]
    [Space(2)]
    private float m_currentTime; //Time between sentences.
    [Tooltip("How much time before the next sentence starts displaying.")]
    public float timeBetweenSentences;
    private bool m_sentenceFinish; //Checks if all characters have been printed.
    [Tooltip("Transition image reference.")]
    public Image transition;
    [Tooltip("Bool to check if the text is paused.")]
    public bool m_pauseText;
    private bool m_isSkipped; //Check if the cutscene is skipped.
    private bool m_sceneSkipped;
    private float m_defaultTypeWriterSpeed; //Set default speed of text print.

    private bool test; //Test bool for bug fix.

    // Start is called before the first frame update
    void Start()
    {
        m_defaultTypeWriterSpeed = typewriterSpeed;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(f_typewriter());
    }

    private void Update()
    {
        var tempColor = transition.color; //Set transition opacity (smoother scene transitions).
        tempColor.a -= 0.2f * Time.deltaTime;
        transition.color = tempColor;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Ending_Scene") && m_stopTextLoop == false)
        {
            test = true;
            typewriterSpeed = m_defaultTypeWriterSpeed;
            m_index = 5;
            m_pauseText = false;
            m_stopTextLoop = true; //Stop the text repeating hundreds of time (stops coroutine being called more than once). 
            m_sentenceFinish = true;
            m_isSkipped = false;
            StartCoroutine(f_typewriter());
        }

        if (m_sentenceFinish == true)
        {
            m_currentTime += Time.deltaTime; //Add to the intermission between sentences.
            if (m_currentTime >= timeBetweenSentences)
            {
                m_sentenceFinish = false;
                m_currentTime = 0; //Reset time.
                m_index++; //Increase index ready for the next sentence.
                cutsceneTextMesh.text = ""; //Show no text.
                StartCoroutine(f_typewriter()); //Begin typing next sentence.
            }
        }

        if (m_index == 5 && m_isSkipped == false && test == false) //Index 5 is null and is used for the switching between game scenes (smoother and more reliable).
        {
            m_isSkipped = true;
            StartCoroutine(f_skipScene()); //Smooth scene transfer (no text shown on screen because of coroutine delay).
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_isSkipped == false && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Opening_Scene"))
        {
            m_index = 5;
            m_isSkipped = true;
            typewriterSpeed = 0.001f;
        }
    }

    IEnumerator f_typewriter()
    {
        if (m_pauseText == false)
        {
            foreach (char letter in cutsceneText[m_index].ToCharArray()) //Grab each individual letter from the current index string.
            {
                cutsceneTextMesh.text += letter; //Add the letter to the current text shown on screen.
                yield return new WaitForSeconds(typewriterSpeed); //Wait before adding another letter.
            }
            if (m_index < cutsceneText.Length) //Check if all characters have been added.
            {
                if (m_isSkipped == false)
                {
                    m_sentenceFinish = true;
                }
                else if(m_isSkipped == true && m_sceneSkipped == false)
                {
                    m_sceneSkipped = true;
                    StartCoroutine(f_skipScene());
                }
            }
            else
            {
                cutsceneTextMesh.text = "";
            }
        }
    }

    IEnumerator f_skipScene()
    {
        m_index = 5;
        cutsceneTextMesh.text = ""; //Create blank text.   
        m_pauseText = true;
        m_sentenceFinish = true;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Game_Scene");
    }

}
