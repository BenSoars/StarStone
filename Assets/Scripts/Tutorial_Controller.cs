using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_Controller : MonoBehaviour
{
    [Header("Required Scripts")]
    private Prototype_Classes m_prototypeClasses;
    private Clock_Controller m_clockController;
    private Wave_System m_waveSystem;

    [Header("Movement Instructions")] //Moving text instructions.
    [Space(2)]
    public TextMeshProUGUI wText;
    public TextMeshProUGUI aText;
    public TextMeshProUGUI sText;
    public TextMeshProUGUI dText;
    public TextMeshProUGUI spaceText;

    [Header("Shooting Instructions")] //Shooting text instructions.
    [Space(2)]
    public TextMeshProUGUI leftClickText;
    public TextMeshProUGUI rightClickText;
    public TextMeshProUGUI middleClickText;

    [Header("Note Instructions")] //Note text instructions.
    [Space(2)]
    public TextMeshProUGUI findNotesText;
    public TextMeshProUGUI tabClickText;
    public TextMeshProUGUI tabExitText;

    [Tooltip("Starstone Instructions")] //Starstone text instructions.
    [Space(2)]
    public TextMeshProUGUI locateStarstoneText;
    public TextMeshProUGUI num1;
    public TextMeshProUGUI num2;
    public TextMeshProUGUI num3;

    [Tooltip("Clock Instructions")] //Clock text instructions.
    [Space(2)]
    public GameObject clockText;
    public TextMeshProUGUI timeToSetText;


    [Tooltip("Bool Variables")] //Bool values to check if certain keys have been pressed.
    private bool m_wPressed;
    private bool m_aPressed;
    private bool m_sPressed;
    private bool m_dPressed;
    private bool m_spacePressed;
    private bool m_leftClickPressed;
    private bool m_rightClickPressed;
    private bool m_scrollWheelPressed;
    private bool m_tabClicked;
    private bool m_1pressed;
    private bool m_2pressed;
    private bool m_3pressed;
    private bool m_notesComplete;

    [Tooltip("Int Variables")] //Int values to check if all buttons have been pressed.
    private int m_WASD;
    private int m_mouseCheck;
    private int m_numbersPressed;

    [Tooltip("UI Game Objects")] //UI game objects for enabling/disabling.
    [Space(2)]
    public GameObject stoneBarrier; 
    public GameObject WASD;
    public GameObject mouseCheck;
    public GameObject note;
    public GameObject noteBarrier;
    public GameObject killEnemiesText;
    public GameObject clockBarrier;
    public GameObject abilityExample;
    public GameObject QorV;
    public GameObject leftClickAbility;
    public GameObject uiDescription; 
    public GameObject noteReleased;
    public GameObject clockPartReleased;
    public GameObject findClock;
    public GameObject packapunch;

    [Tooltip("Where the raycast is shot from.")]
    public Transform shotPoint;

    private void Start()
    {
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        m_clockController = FindObjectOfType<Clock_Controller>();
        m_waveSystem = FindObjectOfType<Wave_System>();

        //Disabled until needed.
        findNotesText.enabled = false;
        tabClickText.enabled = false;
        tabExitText.enabled = false;
        locateStarstoneText.enabled = false;
        killEnemiesText.active = false;
        timeToSetText.enabled = false;
        leftClickAbility.active = false;
        uiDescription.active = false;
        abilityExample.active = false;
    }
    // Update is called once per frame
    void Update()
    {
        f_movementCheck();
        f_shootCheck();
        f_noteCheck();
        f_stoneCheck();
        f_abilityInstructions();

        noteReleased.active = false;
        clockPartReleased.active = false;
        findClock.active = false;
        packapunch.active = false;
    }

    void f_movementCheck()
    {
        if (Input.GetKeyDown("w") && m_wPressed == false) //Detect to see if they are following the tutorial correctly by clicking the correct keys.
        {
            wText.color = Color.green; //Set the text to green to indicate to the player than have followed the tutorial correctly.
            m_wPressed = true; //Stop loop being entered again.
            m_WASD += 1; //Add 1 to the buttons clicked count to detect if they have clicked all buttons required.
        }
        if (Input.GetKeyDown("a") && m_aPressed == false)
        {
            aText.color = Color.green;
            m_aPressed = true;
            m_WASD += 1;
        }
        if (Input.GetKeyDown("s") && m_sPressed == false)
        {
            sText.color = Color.green;
            m_sPressed = true;
            m_WASD += 1;
        }
        if (Input.GetKeyDown("d") && m_dPressed == false)
        {
            dText.color = Color.green;
            m_dPressed = true;
            m_WASD += 1;
        }
        if (Input.GetKeyDown(KeyCode.Space) && m_spacePressed == false)
        {
            spaceText.color = Color.green;
            m_spacePressed = true;
            m_WASD += 1;
        }

        if (m_WASD == 5) //If they have pressed all 5 buttons required >
        {
            WASD.active = false; // > disable the WASD gameobject and >
            mouseCheck.active = true; // move onto the next part of the tutorial.

            m_prototypeClasses.fogStrength = 0; //Set current fog to 0 during tutorial.
        }
    }

    void f_shootCheck()
    {
        if (m_WASD == 5) //Shot the buttons being clicked early.
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && m_leftClickPressed == false) //Check for left button mouse click.
            {
                leftClickText.color = Color.green; //Set to green to indicate it has been clicked.
                m_leftClickPressed = true; //Stop loop.
                m_mouseCheck += 1; //Add 1 to the end check to see if all instructions have been followed.
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && m_rightClickPressed == false)
            {
                rightClickText.color = Color.green;
                m_rightClickPressed = true;
                m_mouseCheck += 1;
            }

            if (Input.GetKeyDown(KeyCode.Mouse2) && m_scrollWheelPressed == false)
            {
                middleClickText.color = Color.green;
                m_scrollWheelPressed = true;
                m_mouseCheck += 1;
            }

            if (m_mouseCheck == 3)
            {
                if (note != null)
                {
                    findNotesText.enabled = true;
                }
                mouseCheck.active = false;
                noteBarrier.active = false;
            }
        }
    }

    void f_noteCheck()
    {
        if (m_mouseCheck == 3) //If all mouse button instructions have been followed then this loop will run.
        {
            if (note == null && findNotesText.enabled == true) //If there is no longer a note (player picked it up) >
            {
                findNotesText.enabled = false; // > turn off the find note text >
                tabClickText.enabled = true; // > and instruct them to click 'TAB'.
            }

            if (Input.GetKeyDown(KeyCode.Tab) && tabClickText.enabled == true) //If the player presses tab >
            {
                m_tabClicked = true; // indicate to the program it has been pressed >
                tabClickText.enabled = false; // > hide the click tab text >
                tabExitText.enabled = true; // > and show the player how to exit the notes system by pressing tab again.
            }

            else if (Input.GetKeyDown(KeyCode.Tab) && m_tabClicked == true)
            {
                stoneBarrier.active = false; //Allow the player 
                m_notesComplete = true; //This bool is true when the player has followed the instructions on how the note system works.
                tabExitText.enabled = false;
            }
        }
    }

    void f_stoneCheck()
    {
        if (m_notesComplete == true) 
        {
            locateStarstoneText.enabled = true; //Display locate starstone text.
        }

        RaycastHit m_rayHit;
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out m_rayHit, 2f))
        {       
           if (m_rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Stones") && Input.GetKeyDown("f") && locateStarstoneText.enabled == true) //If the player presses f then >
            {
                m_waveSystem.canSpawnEnemies = true;
                m_notesComplete = false;
                locateStarstoneText.enabled = false;
                killEnemiesText.active = true; // > indicate to the player that there are enemies that can be killed. 
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && m_1pressed == false) //Check if the player has switched to another weapon.
        {
            num1.color = Color.green; //Change text color to indicate they have done it correctly.
            m_1pressed = true; //Stop loop.
            m_numbersPressed += 1; //Add 1 to numbers pressed during weapon switch tutorial.
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && m_2pressed == false)
        {
            num2.color = Color.green;
            m_2pressed = true;
            m_numbersPressed += 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && m_3pressed == false)
        {
            num3.color = Color.green;
            m_3pressed = true;
            m_numbersPressed += 1;
        }

        if (m_numbersPressed == 3) //If all buttons have been pressed in weapon switching tutorial >
        {
            m_numbersPressed = 4; //Stop loop.
            killEnemiesText.active = false; //Hide find enemy text.
            abilityExample.active = true; //Begin ability usage tutorial.
        }
    }

    void f_abilityInstructions()
    {
        if (abilityExample == true)
        {
            if (QorV.active == true && Input.GetKeyDown("q") || QorV.active == true && Input.GetKeyDown("v")) //Detect if the player has pressed either of the ability activation buttons.
            {
                QorV.active = false; //Hide Q or V instructions.
                leftClickAbility.active = true; //Show left click to use instructions.
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && leftClickAbility.active == true) //If the player uses the ability correctly
            {
                leftClickAbility.active = false;
                Invoke("f_fixClock", 11.5f); //Begin clock tutorial.
                uiDescription.active = true; //Show UI icons description.
            }
        }
    }

    void f_fixClock()
    {
        uiDescription.active = false;
        clockText.active = true;
        timeToSetText.enabled = true;
        timeToSetText.text = "Set time to " + m_clockController.globalHour + ":" + m_clockController.globalMin + ".";
        killEnemiesText.active = false;
        clockBarrier.active = false;
    }
}
