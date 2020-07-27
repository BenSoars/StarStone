using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_End : MonoBehaviour
{
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
        Debug.Log(m_playerEnter);
        var tempColor = transition.color;
        if (m_playerEnter == true && Input.GetKeyDown("f"))
        {
            m_portalController.transitionActive = true;
            Destroy(cutsceneTextCanvas);
            exitPrompt.enabled = false;

        }
        if (startCredits == true)
        {
            anim.SetBool("Active", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            exitPrompt.enabled = true;
            m_playerEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player_End")
        {
            exitPrompt.enabled = false;
            m_playerEnter = false;
        }
    }
}
