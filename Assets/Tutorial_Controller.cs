using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_Controller : MonoBehaviour
{
    private Prototype_Classes m_prototypeClasses;
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

    public TextMeshProUGUI locateClockText;

    private bool m_wPressed;
    private bool m_aPressed;
    private bool m_sPressed;
    private bool m_dPressed;

    private bool m_leftClickPressed;
    private bool m_rightClickPressed;
    private bool m_scrollWheelPressed;

    private bool m_tabPressed;

    private int m_WASD;
    private int m_mouseCheck;

    public GameObject stoneBarrier;
    public GameObject WASD;
    public GameObject mouseCheck;
    public GameObject note;
    public GameObject noteBarrier;

    private void Start()
    {
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        findNotesText.enabled = false;
        tabClickText.enabled = false;
        tabExitText.enabled = false;
        locateClockText.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        f_movementCheck();
        f_shootCheck();
        f_noteCheck();
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
            tabClickText.enabled = false;
            tabExitText.enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.Tab) && tabExitText.enabled == true)
        {

        }
    }
}
