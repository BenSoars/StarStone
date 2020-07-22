﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//Kurtis Watson
public class User_Interface : MonoBehaviour
{
    private Wave_System r_waveSystem;
    private Player_Controller r_playerController;
    private Prototype_Classes r_prototypeClasses;
    private Pickup_System pickupSystem;

    public TMPro.TextMeshProUGUI m_currentTimeText;
    public TMPro.TextMeshProUGUI m_currentHealth;
    public TMPro.TextMeshProUGUI m_currentStoneCharge;
    public TMPro.TextMeshProUGUI timeTillNextRound;
    public TMPro.TextMeshProUGUI chooseStone;
    public TMPro.TextMeshProUGUI interactText;

    public TMPro.TextMeshProUGUI noteSpawnedText;
    public TMPro.TextMeshProUGUI cogSpawnedText;

    public GameObject gameUI;
    public GameObject repairBar;
    public GameObject pauseMenu;
    public GameObject transition;
    public GameObject runtimeUI;
    public GameObject locateClockText;

    public GameObject stone;

    public TMPro.TextMeshPro m_SS1;
    public TMPro.TextMeshPro m_SS2;
    public TMPro.TextMeshPro m_SS3;
    public TMPro.TextMeshPro m_SS4;

    private float m_targetTime;

    private int m_currentSecond;
    private int m_currentMinute;

    private bool pauseMenuActive;
    private bool isLooking;
    private bool isChosen;
    private bool bugFix;
    private bool findClock;

    public List<int> m_waveTimes = new List<int>();

    public Image starstoneIcon;
    public Image abilityPreview1;
    public Image abilityPreview2;
    public Image abilityIcon1;
    public Image abilityIcon2;

    public Sprite[] startstoneIcons;
    public Sprite[] abilityIcons;

    public Camera cameraLook;

    private string currentText;

    private void Start()
    {
        DontDestroyOnLoad(this);
        abilityPreview1.enabled = false;
        abilityPreview2.enabled = false;
        interactText.enabled = false;
        transition.active = false;
        pauseMenu.active = false;
        repairBar.active = false;
        noteSpawnedText.enabled = false;
        cogSpawnedText.enabled = false;
        r_waveSystem = FindObjectOfType<Wave_System>();
        r_playerController = FindObjectOfType<Player_Controller>();
        r_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        pickupSystem = FindObjectOfType<Pickup_System>();
        timeTillNextRound.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        starstoneFunctions();
        interactionText();
        f_pauseMenu();

        m_currentHealth.text = "" + r_playerController.playerHealth.ToString("F0");

        m_currentStoneCharge.text = "" + r_prototypeClasses.stonePower[r_prototypeClasses.classState].ToString("F0");

        m_SS1.text = r_prototypeClasses.stonePower[0].ToString("F0");
        m_SS2.text = r_prototypeClasses.stonePower[1].ToString("F0");
        m_SS3.text = r_prototypeClasses.stonePower[2].ToString("F0");
        m_SS4.text = r_prototypeClasses.stonePower[3].ToString("F0");

        if (r_waveSystem.enemiesLeft == 0 && r_waveSystem.curRound > 0)
        {
            timeTillNextRound.enabled = true;
            if (bugFix == true) {
                bugFix = false;
                stone.GetComponentInChildren<Animator>().SetBool("Chosen", false);
                runtimeUI.active = false;
                isChosen = false;
                chooseStone.enabled = true;
            }
        }
        else
        {
            chooseStone.enabled = false;
            timeTillNextRound.enabled = false;
        }

        if (r_waveSystem.m_startedWaves == true && runtimeUI.active == true)
        {
            m_currentSecond = Mathf.FloorToInt(m_targetTime % 60);
            m_currentMinute = Mathf.FloorToInt(m_targetTime / 60);

            m_targetTime -= Time.deltaTime;

            m_currentTimeText.text = "" + m_currentMinute.ToString("00") + ":" + m_currentSecond.ToString("00");

            if (m_targetTime <= 0 || r_playerController.playerHealth <= 0)
            {
                Destroy(gameObject);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("GameOver");
            }
        }

        switch (r_prototypeClasses.classState)
        {
            case 0:
                abilityIcon1.sprite = abilityIcons[0];
                abilityIcon2.sprite = abilityIcons[1];
                m_currentStoneCharge.color = Color.yellow;
                break;
            case 1:
                abilityIcon1.sprite = abilityIcons[2];
                abilityIcon2.sprite = abilityIcons[3];
                m_currentStoneCharge.color = Color.white;
                break;
            case 2:
                abilityIcon1.sprite = abilityIcons[4];
                abilityIcon2.sprite = abilityIcons[5];
                m_currentStoneCharge.color = Color.magenta;
                break;
            case 3:
                abilityIcon1.sprite = abilityIcons[6];
                abilityIcon2.sprite = abilityIcons[7];
                m_currentStoneCharge.color = Color.blue;
                break;
        }

        timeTillNextRound.text = "NEXT ROUND IN " + r_waveSystem.m_currentIntermissionTime.ToString("F0");
        starstoneIcon.sprite = startstoneIcons[r_prototypeClasses.classState];

    }

    public void f_popupText()
    {
        if (pickupSystem.m_spawnNote == true)
        {
            pickupSystem.m_spawnNote = false;
            noteSpawnedText.enabled = true;
        }
        if (pickupSystem.m_spawnCogs == true)
        {
            if(findClock == false)
            {
                findClock = true;
                locateClockText.active = true;
            }
            pickupSystem.m_spawnCogs = false;
            cogSpawnedText.enabled = true;
        }
        Invoke("f_resetText", 4);
    }

    void f_resetText()
    {
        noteSpawnedText.enabled = false;
        cogSpawnedText.enabled = false;
        locateClockText.active = false;
    }

    public void f_waveTimer()
    {
        m_targetTime = m_waveTimes[r_waveSystem.curRound];
    }

    public void f_pauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuActive = !pauseMenuActive;
        }

        if (pauseMenuActive == true)
        {
            gameUI.active = false;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            gameUI.active = true;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        pauseMenu.active = pauseMenuActive;
    }

    public void resumeButton()
    {
        pauseMenuActive = !pauseMenuActive;
    }

    public void exitButton()
    {
        Destroy(gameObject);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameUI.active = false;
        pauseMenuActive = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void interactionText()
    {
        RaycastHit m_objectHit;

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_objectHit, 100f))
        {
            float distance = Vector3.Distance(cameraLook.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.GetComponent("Interact_Text") as Interact_Text) != null && distance <= 3)
            {
                currentText = m_objectHit.collider.GetComponent<Interact_Text>().text;
                interactText.enabled = true;
                interactText.text = currentText;
            }
            else
            {
                interactText.enabled = false;
            }
        }
    }

    public void starstoneFunctions()
    {
        RaycastHit m_objectHit;

        if (Physics.Raycast(cameraLook.transform.position, cameraLook.transform.forward, out m_objectHit, 50f))
        {
            float distance = Vector3.Distance(cameraLook.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.GetComponent("Starstone_ID") as Starstone_ID) && distance <= 3 && r_prototypeClasses.canSelect == true)
            {
                isLooking = true;
                stone = m_objectHit.collider.gameObject;
                abilityPreview1.sprite = m_objectHit.collider.GetComponent<Starstone_ID>().preview1;
                abilityPreview2.sprite = m_objectHit.collider.GetComponent<Starstone_ID>().preview2;
                abilityPreview1.enabled = true;
                abilityPreview2.enabled = true;
            }
            else
            {
                isLooking = false;
                abilityPreview1.enabled = false;
                abilityPreview2.enabled = false;
            }
            if (Input.GetKeyDown("f"))
            {
                Invoke("f_bugFix", 10);
                isLooking = false;
                stone.GetComponentInChildren<Animator>().SetBool("Chosen", true);
            }
            stone.GetComponentInChildren<Animator>().SetBool("Looking", isLooking);
        }
    }

    void f_bugFix()
    {
        bugFix = true;
    }
}