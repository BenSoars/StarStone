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
    public int timeBeforeCredits = 4; //Seconds before the credits begin scrolling.
    private Portal_Controller m_portalController; //Reference portal controller script.
    public TextMeshProUGUI exitPrompt; //Exit temple message.
    [Tooltip("Check to see if the transition is active or not.")]
    public bool transitionActive; //Start transition between screen.
    private bool m_playerEnter;
    [Tooltip("Animator for the transition.")]
    public Animator anim; //Reference game object animator.
    [Tooltip("Transition image.")]
    public Image transition;
    [Tooltip("Canvas that holds the content for the cutscene text.")]
    public GameObject cutsceneTextCanvas;
    public Image test; //Blank variable.
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
