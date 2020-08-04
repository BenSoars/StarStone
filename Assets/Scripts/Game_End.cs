using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_End : MonoBehaviour
{
    [Header("Ending Cutscene Components")]
    [Space(2)]
    [Tooltip("Set the time before credits begin to scroll.")]
    public int timeBeforeCredits = 4;
    private Portal_Controller m_portalController;
    public TextMeshProUGUI exitPrompt;
    public bool transitionActive;
    private bool m_playerEnter;
    public Animator anim;
    public Image transition;
    public GameObject cutsceneTextCanvas;
    public Image test;
    public Image background;

    void Start()
    {
        background.enabled = false;
        exitPrompt.enabled = false;
        cutsceneTextCanvas = GameObject.Find("Cutscene Text");
        m_portalController = FindObjectOfType<Portal_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        var tempColor = transition.color; //Access the opacity of the transition object.
        if (m_playerEnter == true && Input.GetKeyDown("f"))
        {
            m_portalController.transitionActive = true; //Start transition to black screen.
            Destroy(cutsceneTextCanvas); //Destroy the cutscene text canvas.
            exitPrompt.enabled = false; //Disable exit prompt.
            Invoke("f_startCredits", timeBeforeCredits);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Active") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SceneManager.LoadScene("Main_Menu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        exitPrompt.enabled = true; //Display exit instructions.
        m_playerEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
            exitPrompt.enabled = false; //Hide exit instructions.
            m_playerEnter = false;
    }

    void f_startCredits()
    {
        anim.SetBool("Active", true);
        background.enabled = true;
        Destroy(test);
    }
}
