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
    public TextMeshProUGUI cutsceneTextMesh;
    public string[] cutsceneText;
    private int m_index;
    public float typewriterSpeed;
    private bool m_stopTextLoop;

    [Header("Cutscene Components")]
    [Space(2)]
    public Image shrine;
    private float m_currentTime;
    public float timeBetweenSentences;
    private bool m_sentenceFinish;
    public Image transition;
    public bool m_pauseText;
    private bool m_isSkipped;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(f_typewriter());
    }

    private void Update()
    {
        var tempColor = transition.color; //Set transition opacity (smoother scene transitions).
        tempColor.a -= 0.2f * Time.deltaTime;
        transition.color = tempColor;

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

        if(m_index == 5 && m_isSkipped == false) //Index 5 is null and is used for the switching between game scenes (smoother and more reliable).
        {
            m_isSkipped = true;
            StartCoroutine(f_skipScene()); //Smooth scene transfer (no text shown on screen because of coroutine delay).
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_isSkipped == false)
        {
            m_isSkipped = true;
            StartCoroutine(f_skipScene());
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Ending_Scene") && m_stopTextLoop == false)
        {
            m_index = 5;
            m_pauseText = false;
            m_stopTextLoop = true; //Stop the text repeating hundreds of time (stops coroutine being called more than once). 
            m_sentenceFinish = true;
            StartCoroutine(f_typewriter());                    
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
                m_sentenceFinish = true;
            }
            else
            {
                cutsceneTextMesh.text = "";
            }
        }
    }

    IEnumerator f_skipScene()
    {
        cutsceneTextMesh.text = ""; //Create blank text.   
        m_pauseText = true;
        m_isSkipped = true;  
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Game_Scene");
    }
}
