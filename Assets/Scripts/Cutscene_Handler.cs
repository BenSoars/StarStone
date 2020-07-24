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

    [Header("Cutscene Components")]
    [Space(2)]
    public Image shrine;
    private float m_currentTime;
    public float timeBetweenSentences;
    private bool m_sentenceFinish;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(f_typewriter());
    }

    private void Update()
    {
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
                if (m_index == 5) //If the index is 5 (finished first cutscene) then load the game scene.
                {
                    SceneManager.LoadScene("Game_Scene");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) //Skip the cutscene entirely.
        {
            SceneManager.LoadScene("Game_Scene");
        }
    }

    IEnumerator f_typewriter()
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
