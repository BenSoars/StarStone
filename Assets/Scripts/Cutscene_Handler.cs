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
    public TextMeshProUGUI cutsceneTextMesh;

    public string[] cutsceneText;
    
    private int index;
    public float typewriterSpeed;

    private float currentTime;
    public float timeBetweenSentences;

    private bool sentenceFinish;
    private bool pauseCutscene;

    public Image shrine;
    public Image COD;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(f_typewriter());
        COD.enabled = false;
    }

    private void Update()
    {
        if (sentenceFinish == true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeBetweenSentences)
            {
                sentenceFinish = false;
                currentTime = 0;
                index++;
                cutsceneTextMesh.text = "";
                StartCoroutine(f_typewriter());
                if(index == 5)
                {
                    SceneManager.LoadScene("Temple_Kurt");
                }
            }
        }

        //bool test = false;
        //if(index == 4 && test == false)
        //{            
        //    Debug.Log("Paused");
        //    test = true;
        //    pauseCutscene = true;
        //}

        if (Input.GetKeyDown("g"))
        {
            shrine.enabled = false;
            COD.enabled = true;
            pauseCutscene = false;
            StartCoroutine(f_typewriter());
        }
    }

    IEnumerator f_typewriter()
    {
        if (pauseCutscene == false)
        {
            foreach (char letter in cutsceneText[index].ToCharArray())
            {
                cutsceneTextMesh.text += letter;
                yield return new WaitForSeconds(typewriterSpeed);
            }
            if (index < cutsceneText.Length)
            {
                sentenceFinish = true;
            }
            else
            {
                cutsceneTextMesh.text = "";
            }
        }
    }
}
