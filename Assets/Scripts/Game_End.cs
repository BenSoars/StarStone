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
    private Portal_Controller m_portalController;
    public TextMeshProUGUI exitPrompt;
    public bool transitionActive;
    public bool startCredits;
    private bool m_playerEnter;
    private Animator anim;
    public Image transition;
    public GameObject cutsceneTextCanvas;

    void Start()
    {
        exitPrompt.enabled = false;
        anim = GetComponentInChildren<Animator>();
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

        }
        if (startCredits == true)
        {
            anim.SetBool("Active", true); //Begin scrolling credits.
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
}
