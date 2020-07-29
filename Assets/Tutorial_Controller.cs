using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_Controller : MonoBehaviour
{
    private Prototype_Classes m_prototypeClasses;
    private Clock_Controller m_clockController;

    public TextMeshProUGUI wText;
    public TextMeshProUGUI aText;
    public TextMeshProUGUI sText;
    public TextMeshProUGUI dText;

    public TextMeshProUGUI leftClickText;
    public TextMeshProUGUI rightClickText;
    public TextMeshProUGUI middleClickText;

    public TextMeshProUGUI findNotesText;
    public TextMeshProUGUI tabClickText;
    public TextMeshProUGUI tabExitText;

    public TextMeshProUGUI locateStarstoneText;

    public TextMeshProUGUI fixClockText;
    public TextMeshProUGUI timeToSetText;

    public TextMeshProUGUI num1;
    public TextMeshProUGUI num2;
    public TextMeshProUGUI num3;

    private bool m_wPressed;
    private bool m_aPressed;
    private bool m_sPressed;
    private bool m_dPressed;

    private bool m_leftClickPressed;
    private bool m_rightClickPressed;
    private bool m_scrollWheelPressed;

    private bool m_tabClicked;

    private bool m_1pressed;
    private bool m_2pressed;
    private bool m_3pressed;

    private int m_WASD;
    private int m_mouseCheck;
    private int m_numbersPressed;

    public GameObject stoneBarrier;
    public GameObject WASD;
    public GameObject mouseCheck;
    public GameObject note;
    public GameObject noteBarrier;
    public GameObject killEnemiesText;
    public GameObject clockBarrier;

    private bool notesComplete;

    private void Start()
    {
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        m_clockController = FindObjectOfType<Clock_Controller>();

        findNotesText.enabled = false;
        tabClickText.enabled = false;
        tabExitText.enabled = false;
        locateStarstoneText.enabled = false;
        killEnemiesText.active = false;
        fixClockText.enabled = false;
        timeToSetText.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        f_movementCheck();
        f_shootCheck();
        f_noteCheck();
        f_stoneCheck();
        if (m_mouseCheck == 3)
        {
            m_prototypeClasses.canSelect = true;
        }
        else m_prototypeClasses.canSelect = false;
    }

    void f_movementCheck()
    {
        if (Input.GetKeyDown("w") && m_wPressed == false)
        {
            wText.color = Color.green;
            m_wPressed = true;
            m_WASD += 1;
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

        if (m_WASD == 4)
        {
            WASD.active = false;
            mouseCheck.active = true;

            m_prototypeClasses.canSelect = true;
            m_prototypeClasses.fogStrength = 0; //Set current fog.
        }
    }

    void f_shootCheck()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && m_leftClickPressed == false)
        {
            leftClickText.color = Color.green;
            m_leftClickPressed = true;
            m_mouseCheck += 1;
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

        if(m_mouseCheck == 3)
        {
            if (note != null)
            {
                findNotesText.enabled = true;
            }
            mouseCheck.active = false;
            noteBarrier.active = false;      
        }
    }

    void f_noteCheck()
    {
        if (note == null && findNotesText.enabled == true)
        {
            findNotesText.enabled = false;
            tabClickText.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && tabClickText.enabled == true)
        {
            m_tabClicked = true;
            tabClickText.enabled = false;
            tabExitText.enabled = true;
        }

        else if(Input.GetKeyDown(KeyCode.Tab) && m_tabClicked == true)
        {
            stoneBarrier.active = false;
            notesComplete = true;
            tabExitText.enabled = false;
        }
    }

    void f_stoneCheck()
    {
        if(notesComplete == true)
        {            
            locateStarstoneText.enabled = true;
        }
        if(Input.GetKeyDown("f") && locateStarstoneText.enabled == true)
        {
            notesComplete = false;
            locateStarstoneText.enabled = false;
            killEnemiesText.active = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && m_1pressed == false)
        {
            num1.color = Color.green;
            m_1pressed = true;
            m_numbersPressed += 1;
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

        if(m_numbersPressed == 3)
        {
            fixClockText.enabled = true;
            timeToSetText.enabled = true;
            timeToSetText.text = "Set time to " + m_clockController.globalHour + ":" + m_clockController.globalMin + "."; 
            killEnemiesText.active = false;
            clockBarrier.active = false;
        }
    }
}
