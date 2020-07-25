using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Security.Cryptography;

//Kurtis Watson
public class User_Interface : MonoBehaviour
{
    [Header("Script References")]
    private Wave_System m_waveSystem;
    private Player_Controller m_playerController;
    private Prototype_Classes m_prototypeClasses;
    private Pickup_System m_pickupSystem;

    [Header("Text Mesh Pro UGUI")]
    [Space(2)]

    public TMPro.TextMeshProUGUI currentTimeText;
    public TMPro.TextMeshProUGUI currentHealth;
    public TMPro.TextMeshProUGUI currentStoneCharge;
    public TMPro.TextMeshProUGUI timeTillNextRound;
    public TMPro.TextMeshProUGUI chooseStone;
    public TMPro.TextMeshProUGUI interactText;
    public TMPro.TextMeshProUGUI noteSpawnedText;
    public TMPro.TextMeshProUGUI cogSpawnedText;

    [Header("UI Game Objects")]
    [Space(2)]
    public GameObject gameUI;
    public GameObject repairBar;
    public GameObject pauseMenu;
    public GameObject transition;
    public GameObject runtimeUI;
    public GameObject locateClockText;
    public GameObject stone;
    public GameObject difficultyHandler;

    [Header("In-game Individual Canvas")]
    public TMPro.TextMeshPro SS1;
    public TMPro.TextMeshPro SS2;
    public TMPro.TextMeshPro SS3;
    public TMPro.TextMeshPro SS4;

    [Header("UI Images")]
    public Image starstoneIcon;
    public Image abilityPreview1;
    public Image abilityPreview2;
    public Image abilityIcon1;
    public Image abilityIcon2;
    public Sprite[] startstoneIcons;
    public Sprite[] abilityIcons;

    [Header("Runtime Components")]
    public Camera camera;
    private string m_currentInteractionText;      
    private float m_targetTime;
    private int m_currentSecond;
    private int m_currentMinute;
    private bool m_pauseMenuActive;
    private bool m_isLooking;
    private bool m_bugFix;
    private bool m_findClock;
    public List<int> waveTimes = new List<int>();

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
        m_waveSystem = FindObjectOfType<Wave_System>();
        m_playerController = FindObjectOfType<Player_Controller>();
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        m_pickupSystem = FindObjectOfType<Pickup_System>();
        timeTillNextRound.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        starstoneFunctions();
        interactionText();
        f_pauseMenu();

        currentHealth.text = "" + m_playerController.playerHealth.ToString("F0");

        SS1.text = m_prototypeClasses.stonePower[0].ToString("F0");
        SS2.text = m_prototypeClasses.stonePower[1].ToString("F0");
        SS3.text = m_prototypeClasses.stonePower[2].ToString("F0");
        SS4.text = m_prototypeClasses.stonePower[3].ToString("F0");

        if (m_waveSystem.enemiesLeft == 0 && m_waveSystem.curRound > 0)
        {
            timeTillNextRound.enabled = true;
            if (m_bugFix == true) {
                m_bugFix = false;
                stone.GetComponentInChildren<Animator>().SetBool("Chosen", false);
                runtimeUI.active = false;
                chooseStone.enabled = true;
            }
        }
        else
        {
            chooseStone.enabled = false;
            timeTillNextRound.enabled = false;
        }

        if (m_waveSystem.m_startedWaves == true && runtimeUI.active == true)
        {
            m_currentSecond = Mathf.FloorToInt(m_targetTime % 60);
            m_currentMinute = Mathf.FloorToInt(m_targetTime / 60);

            m_targetTime -= Time.deltaTime;

            currentTimeText.text = "" + m_currentMinute.ToString("00") + ":" + m_currentSecond.ToString("00");

            if (m_targetTime <= 0 || m_playerController.playerHealth <= 0)
            {
                Destroy(gameObject);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("GameOver");
            }
        }

        switch (m_prototypeClasses.classState)
        {
            case 0:
                abilityIcon1.sprite = abilityIcons[0];
                abilityIcon2.sprite = abilityIcons[1];
                currentStoneCharge.color = Color.yellow;
                break;
            case 1:
                abilityIcon1.sprite = abilityIcons[2];
                abilityIcon2.sprite = abilityIcons[3];
                currentStoneCharge.color = Color.white;
                break;
            case 2:
                abilityIcon1.sprite = abilityIcons[4];
                abilityIcon2.sprite = abilityIcons[5];
                currentStoneCharge.color = Color.magenta;
                break;
            case 3:
                abilityIcon1.sprite = abilityIcons[6];
                abilityIcon2.sprite = abilityIcons[7];
                currentStoneCharge.color = Color.blue;
                break;
        }

        timeTillNextRound.text = "NEXT ROUND IN " + m_waveSystem.m_currentIntermissionTime.ToString("F0");
        if (m_waveSystem.curRound > 0)
        {
            currentStoneCharge.text = "" + m_prototypeClasses.stonePower[m_prototypeClasses.classState].ToString("F0");
            starstoneIcon.sprite = startstoneIcons[m_prototypeClasses.classState];
        }
        

    }

    public void f_popupText()
    {
        if (m_pickupSystem.spawnNote == true)
        {
            m_pickupSystem.spawnNote = false;
            noteSpawnedText.enabled = true;
        }
        if (m_pickupSystem.spawnCogs == true)
        {
            if(m_findClock == false)
            {
                m_findClock = true;
                locateClockText.active = true;
            }
            m_pickupSystem.spawnCogs = false;
            cogSpawnedText.enabled = true;
        }
        Invoke("f_resetText", 4);
    }

    public void f_resetText()
    {
        noteSpawnedText.enabled = false;
        cogSpawnedText.enabled = false;
        locateClockText.active = false;
    }

    public void f_waveTimer()
    {
        m_targetTime = waveTimes[m_waveSystem.curRound];
    }

    public void f_pauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_pauseMenuActive = !m_pauseMenuActive;
        }

        if (m_pauseMenuActive == true)
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

        pauseMenu.active = m_pauseMenuActive;
    }

    public void resumeButton()
    {
        m_pauseMenuActive = !m_pauseMenuActive;
    }

    public void exitButton()
    {
        Destroy(gameObject);
        Destroy(difficultyHandler);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameUI.active = false;
        m_pauseMenuActive = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void interactionText()
    {
        RaycastHit m_objectHit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_objectHit, 100f))
        {
            float distance = Vector3.Distance(camera.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.GetComponent("Interact_Text") as Interact_Text) != null && distance <= 3)
            {
                m_currentInteractionText = m_objectHit.collider.GetComponent<Interact_Text>().text;
                interactText.enabled = true;
                interactText.text = m_currentInteractionText;
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

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_objectHit, 50f))
        {
            float distance = Vector3.Distance(camera.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.GetComponent("Starstone_ID") as Starstone_ID) && distance <= 3 && m_prototypeClasses.canSelect == true)
            {
                m_isLooking = true;
                stone = m_objectHit.collider.gameObject;
                abilityPreview1.sprite = m_objectHit.collider.GetComponent<Starstone_ID>().preview1;
                abilityPreview2.sprite = m_objectHit.collider.GetComponent<Starstone_ID>().preview2;
                abilityPreview1.enabled = true;
                abilityPreview2.enabled = true;
            }
            else
            {
                m_isLooking = false;
                abilityPreview1.enabled = false;
                abilityPreview2.enabled = false;
            }
            if (Input.GetKeyDown("f") && m_objectHit.collider.gameObject.GetComponent("Starstone_ID") as Starstone_ID)
            {
                Invoke("f_m_bugFix", 10);
                m_isLooking = false;

                stone.GetComponentInChildren<Animator>().SetBool("Chosen", true);
            }
            stone.GetComponentInChildren<Animator>().SetBool("Looking", m_isLooking);
        }
    }

    public void f_m_bugFix()
    {
        m_bugFix = true;
    }
}