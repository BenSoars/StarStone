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
    [Tooltip("Current time left before round end.")]
    public TMPro.TextMeshProUGUI currentTimeText;
    [Tooltip("Current Starstone charge.")]
    public TMPro.TextMeshProUGUI currentStoneCharge;
    [Tooltip("Intermission time.")]
    public TMPro.TextMeshProUGUI timeTillNextRound;
    [Tooltip("Alert the player to select a new Starstone.")]
    public TMPro.TextMeshProUGUI chooseStone;
    [Tooltip("Text that is displayed when a player is looking at an interactable object.")]
    public TMPro.TextMeshProUGUI interactText;
    [Tooltip("Alert the player a note has spawned.")]
    public TMPro.TextMeshProUGUI noteSpawnedText;
    [Tooltip("Alert the player a clock part has spawned.")]
    public TMPro.TextMeshProUGUI cogSpawnedText;
    [Tooltip("Display the amount of clock parts left to collect.")]
    public TMPro.TextMeshProUGUI clockPartsLeft;
    [Tooltip("Display the amount of notes left to collect.")]
    public TMPro.TextMeshProUGUI notesLeft;

    [Header("UI Game Objects")]
    [Space(2)]
    [Tooltip("Game UI reference.")]
    public GameObject gameUI;
    [Tooltip("Repair bar reference.")]
    public GameObject repairBar;
    [Tooltip("Pause menu reference.")]
    public GameObject pauseMenu;
    [Tooltip("During gametime UI reference.")]
    public GameObject runtimeUI;
    [Tooltip("Alert the player to find the clock.")]
    public GameObject locateClockText;
    [Tooltip("Store the current starstone the player is looking at here.")]
    public GameObject stone;
    [Tooltip("Reference the difficulty handler.")]
    public GameObject difficultyHandler;
    public GameObject timeLeft;
    public GameObject leaveNow;

    [Header("UI Images")]
    [Tooltip("Display current starstone image on UI.")]
    public Image starstoneIcon;
    [Tooltip("Show the allogated Q ability on screen.")]
    public Image abilityPreview1;
    [Tooltip("Show the allogated V ability on screen.")]
    public Image abilityPreview2;
    [Tooltip("Show the chosen Q ability on screen,")]
    public Image abilityIcon1;
    [Tooltip("Show the chosen V ability on screen.")]
    public Image abilityIcon2;
    [Tooltip("Health bar on screen.")]
    public Image healthBar;
    [Tooltip("Stores the different sprites used for the starstone icons on the runtime UI.")]
    public Sprite[] startstoneIcons;
    [Tooltip("Stores the different sprites for the starstone ability icons on the runtime UI.")]
    public Sprite[] abilityIcons;
    public Image bloodRed;

    [Header("Runtime Components")]
    [Tooltip("Camera reference.")]
    public Camera camera;
    private string m_interactionText;      
    private float m_targetTime;
    private int m_currentSecond;
    private int m_currentMinute;
    private bool m_pauseMenuActive;
    private bool m_isLooking;
    private bool m_bugFix;
    private bool m_findClock;
    [Tooltip("Stores the wave times for each round.")]
    public List<int> waveTimes = new List<int>();
    [Tooltip("Stores the amount of notes the player has collected.")]
    public int collectedNotes;
    public Animator bloodAnimation;

    //[Header("In-game Individual Canvas")] //This was to display the starstone power in game.
    //public TMPro.TextMeshPro SS1;
    //public TMPro.TextMeshPro SS2;
    //public TMPro.TextMeshPro SS3;
    //public TMPro.TextMeshPro SS4;

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
        f_playerHealth();

        if(m_waveSystem.curRound == 12) { leaveNow.active = true; } //Alert the player they need to leave the temple.
        if(m_waveSystem.curRound >= 13) { m_playerController.playerHealth -= 0.001f; } //Cause the player to forcefully die.

    }

    private void f_playerHealth() //This sets the animation speed based on players health.
    {
        if (m_playerController.playerHealth > 60) //Hide animation if players health is above 60.
        {
            bloodRed.enabled = false;
        }
        else bloodRed.enabled = true;

        if (m_playerController.playerHealth <= 60 && m_playerController.playerHealth > 40) //If the players health is between 2 values, it sets the speed accordingly.
        {
            bloodAnimation.speed = 1;
        }
        if (m_playerController.playerHealth <= 40 && m_playerController.playerHealth > 20)
        {
            bloodAnimation.speed = 2f;
        }
        if (m_playerController.playerHealth <= 20 && m_playerController.playerHealth >= 0)
        {
            bloodAnimation.speed = 3f;
        }
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
        #region Starstone Animation Handler and Intermission Time
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
        #endregion

        #region Current Wave Time
        if (m_waveSystem.m_startedWaves == true && runtimeUI.active == true)
        {
            m_currentSecond = Mathf.FloorToInt(m_targetTime % 60); //Calculate time left into minute/hour readable format.
            m_currentMinute = Mathf.FloorToInt(m_targetTime / 60);

            m_targetTime -= Time.deltaTime; //Decrease current time.

            currentTimeText.text = "" + m_currentMinute.ToString("00") + ":" + m_currentSecond.ToString("00"); //Display global time as readable clock format.

            if (m_targetTime <= 0 || m_playerController.playerHealth <= 0) //Checks if player has either died or ran out of time.
            {
                Animator playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
                playerAnim.SetBool("Dead", true);
                Destroy(gameObject); //Delete the UI from the scene.
                Cursor.lockState = CursorLockMode.None; //Unlock the cursor.
                Cursor.visible = true; //Make cursor visible.
                m_playerController.canPlayerMove = false;
            }
        }
        #endregion

        if (m_waveSystem.curRound > 0)
        {
            currentStoneCharge.text = "" + m_prototypeClasses.stonePower[m_prototypeClasses.classState].ToString("F0"); //Display starstone charge on runtime UI.
            starstoneIcon.sprite = startstoneIcons[m_prototypeClasses.classState]; //Display correct starstone image (bottom right).
        }

        #region State Indicator
        if (m_prototypeClasses.stateQ == true) //Change colour of ability icons on the UI screen to indicate to the player which ability is activated.
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
        #endregion

        timeTillNextRound.text = "NEXT ROUND IN " + m_waveSystem.currentIntermissionTime.ToString("F0"); //Display intermission time.
        healthBar.fillAmount = m_playerController.playerHealth / 100; //Set health bar fill amount based on player health.
        clockPartsLeft.text = m_pickupSystem.repairedParts + " / 5"; //Display collected clock parts on the UI.
        notesLeft.text = collectedNotes + " / 3"; //Display the total amount of collected notes on the UI.

        //SS1.text = m_prototypeClasses.stonePower[0].ToString("F0"); //This was for testing purposes (to show the startsone power as text).
        //SS2.text = m_prototypeClasses.stonePower[1].ToString("F0");
        //SS3.text = m_prototypeClasses.stonePower[2].ToString("F0");
        //SS4.text = m_prototypeClasses.stonePower[3].ToString("F0");
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
        Destroy(difficultyHandler.gameObject); //Destroy difficulty handler so the values can be reset.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameUI.active = false;
        m_pauseMenuActive = false;
        SceneManager.LoadScene("Main_Menu"); //Load main menu scene.
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