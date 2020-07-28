using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;

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
    public TMPro.TextMeshProUGUI clockPartsLeft;
    public TMPro.TextMeshProUGUI notesLeft;

    [Header("UI Game Objects")]
    [Space(2)]
    public GameObject gameUI;
    public GameObject repairBar;
    public GameObject pauseMenu;
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
    public Image healthBar;
    public Sprite[] startstoneIcons;
    public Sprite[] abilityIcons;   

    [Header("Runtime Components")]
    public Camera camera;
    private string m_interactionText;      
    private float m_targetTime;
    private int m_currentSecond;
    private int m_currentMinute;
    private bool m_pauseMenuActive;
    private bool m_isLooking;
    private bool m_bugFix;
    private bool m_findClock;
    public List<int> waveTimes = new List<int>();
    public int collectedNotes;

    private void Start()
    {
        //DontDestroyOnLoad(this);
        abilityPreview1.enabled = false;
        abilityPreview2.enabled = false;
        interactText.enabled = false;
        //transition.active = false;
        pauseMenu.active = false;
        repairBar.active = false;
        chooseStone.enabled = false;
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
        f_starstoneFunctions();
        f_interactionText();
        f_pauseMenu();
        f_setStarstoneAbilityImages();
        f_runtimeUI();
    }

    public void f_setStarstoneAbilityImages()
    {
        switch (m_prototypeClasses.classState) //Grab the chosen class state >
        {
            case 0:
                abilityIcon1.sprite = abilityIcons[0]; //and set the UI images accordingle based on the abilities assigned to that specific statstone.
                abilityIcon2.sprite = abilityIcons[1];
                currentStoneCharge.color = Color.yellow; //Change colour of the startsone on the UI to match the designated colour.
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

    }

    public void f_runtimeUI()
    {
        //SS1.text = m_prototypeClasses.stonePower[0].ToString("F0"); //This was for testing purposes (to show the startsone power as text).
        //SS2.text = m_prototypeClasses.stonePower[1].ToString("F0");
        //SS3.text = m_prototypeClasses.stonePower[2].ToString("F0");
        //SS4.text = m_prototypeClasses.stonePower[3].ToString("F0");

        if (m_waveSystem.enemiesLeft == 0 && m_waveSystem.curRound > 0)
        {
            timeTillNextRound.enabled = true; //Enable the time till next round text gameobject (intermission text).
            if (m_bugFix == true)
            {
                m_bugFix = false;
                stone.GetComponentInChildren<Animator>().SetBool("Chosen", false); //Reset chosen starstone animation to idle.
                runtimeUI.active = false; //Disable game UI.
                chooseStone.enabled = true; //Enable 'Choose Stone' text.
            }
        }
        else
        {
            chooseStone.enabled = false;
            timeTillNextRound.enabled = false;
        }

        if (m_waveSystem.m_startedWaves == true && runtimeUI.active == true)
        {
            m_currentSecond = Mathf.FloorToInt(m_targetTime % 60); //Calculate time left into minute/hour readable format.
            m_currentMinute = Mathf.FloorToInt(m_targetTime / 60);

            m_targetTime -= Time.deltaTime; //Decrease current time.

            currentTimeText.text = "" + m_currentMinute.ToString("00") + ":" + m_currentSecond.ToString("00"); //Display global time as readable clock format.

            if (m_targetTime <= 0 || m_playerController.playerHealth <= 0) //Checks if player has either died or ran out of time.
            {
                Destroy(gameObject); //Delete the UI from the scene.
                Cursor.lockState = CursorLockMode.None; //Unlock the cursor.
                Cursor.visible = true; //Make cursor visible.
                SceneManager.LoadScene("GameOver"); //Load the final game scene.
            }
        }

        timeTillNextRound.text = "NEXT ROUND IN " + m_waveSystem.currentIntermissionTime.ToString("F0"); //Display intermission time.

        if (m_waveSystem.curRound > 0)
        {
            currentStoneCharge.text = "" + m_prototypeClasses.stonePower[m_prototypeClasses.classState].ToString("F0"); //Display starstone charge on runtime UI.
            starstoneIcon.sprite = startstoneIcons[m_prototypeClasses.classState]; //Display correct starstone image (bottom right).
        }

        healthBar.fillAmount = m_playerController.playerHealth / 100; //Set health bar fill amount based on player health.


        if(m_prototypeClasses.stateQ == true) //Change colour of ability icons on the UI screen to indicate to the player which ability is activated.
        {
            abilityIcon1.color = Color.red;
        }
        else if (m_prototypeClasses.stateV == true)
        {
            abilityIcon2.color = Color.red;
        }
        else
        {
            abilityIcon1.color = Color.white;
            abilityIcon2.color = Color.white;
        }

        clockPartsLeft.text = m_pickupSystem.repairedParts + " / 5";
        notesLeft.text = collectedNotes + " / 3";
    }

    public void f_popupText()
    {
        if (m_pickupSystem.spawnNote == true) //Checks if the game has spawned a note >
        {
            m_pickupSystem.spawnNote = false; //Stop loop.
            noteSpawnedText.enabled = true; //if so display the note spawned alert.
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
        Invoke("f_resetText", 4); //Disable text.
    }

    public void f_resetText()
    {
        noteSpawnedText.enabled = false; //Hide text.
        cogSpawnedText.enabled = false;
        locateClockText.active = false;
    }

    public void f_waveTimer()
    {
        m_targetTime = waveTimes[m_waveSystem.curRound]; //Set a new target time, ready for when the next round begins.
    }

    public void f_pauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_pauseMenuActive = !m_pauseMenuActive;
        }

        if (m_pauseMenuActive == true) //Enable the pause menu.
        {
            gameUI.active = false; //Disable game UI.
            Time.timeScale = 0; //Set game timescale to 0.
            Cursor.lockState = CursorLockMode.None; //Unlock cursor.
            Cursor.visible = true;
        }
        else
        {
            gameUI.active = true;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        pauseMenu.active = m_pauseMenuActive; //Pause menu will either show or hide based on bool value.
    }

    public void resumeButton()
    {
        m_pauseMenuActive = !m_pauseMenuActive; //Resume game button.
    }

    public void exitButton()
    {
        Destroy(gameObject); //Destroy game object.
        Destroy(difficultyHandler); //Destroy difficulty handler so the values can be reset.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameUI.active = false;
        m_pauseMenuActive = false;
        SceneManager.LoadScene("MainMenu"); //Load main menu scene.
    }

    public void f_interactionText()
    {
        RaycastHit m_objectHit; //Create a raycast.

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_objectHit, 100f)) //Shoot raycast in direction of where player is looking.
        {
            float distance = Vector3.Distance(camera.transform.position, m_objectHit.collider.transform.position); //Check distance between the player and the object hit.
            if ((m_objectHit.collider.gameObject.GetComponent("Interact_Text") as Interact_Text) != null && distance <= 3) //Check if the collided object holds the 'Interact_Text' script.
            {
                m_interactionText = m_objectHit.collider.GetComponent<Interact_Text>().text; //Grab the string value stored on the object and >
                interactText.enabled = true;
                interactText.text = m_interactionText; // > set that text as the displayed interaction text.
            }
            else
            {
                interactText.enabled = false; //If not colliders, hide text.
            }
        }
    }

    public void f_starstoneFunctions()
    {
        RaycastHit m_objectHit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_objectHit, 50f))
        {
            float distance = Vector3.Distance(camera.transform.position, m_objectHit.collider.transform.position);
            if ((m_objectHit.collider.gameObject.GetComponent("Starstone_ID") as Starstone_ID) && distance <= 3 && m_prototypeClasses.canSelect == true) //Check for object holding 'Starstone_ID' script.
            {
                m_isLooking = true; //Set the starstone the player is looking at to 'Looking' animation state.
                stone = m_objectHit.collider.gameObject;
                abilityPreview1.sprite = m_objectHit.collider.GetComponent<Starstone_ID>().preview1; //Set the ability preview on the UI as that stored in the colliders image variable.
                abilityPreview2.sprite = m_objectHit.collider.GetComponent<Starstone_ID>().preview2;
                abilityPreview1.enabled = true; //Display the ability previews when the player is looking at a starstone >
                abilityPreview2.enabled = true;
            }
            else
            {
                m_isLooking = false;
                abilityPreview1.enabled = false; // > and hide if they look away.
                abilityPreview2.enabled = false;
            }
            if (Input.GetKeyDown("f") && m_objectHit.collider.gameObject.GetComponent("Starstone_ID") as Starstone_ID) //Check if the player selects a starstone.
            {
                Invoke("f_bugFix", 10);
                m_isLooking = false;

                stone.GetComponentInChildren<Animator>().SetBool("Chosen", true); //Set the selected starstone to is 'Chosen' animation state.
            }
            stone.GetComponentInChildren<Animator>().SetBool("Looking", m_isLooking);
        }
    }

    public void f_bugFix()
    {
        m_bugFix = true;
    }

}