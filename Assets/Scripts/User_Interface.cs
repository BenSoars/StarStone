using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Kurtis Watson
public class User_Interface : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_currentTimeText;
    public TMPro.TextMeshProUGUI m_currentHealth;
    public TMPro.TextMeshProUGUI m_currentStoneCharge;
    public TMPro.TextMeshProUGUI timeTillNextRound;
    public TMPro.TextMeshProUGUI chooseStone;

    public GameObject gameUI;
    public GameObject repairBar;
    public GameObject pauseMenu;

    public TMPro.TextMeshProUGUI noteSpawnedText;
    public TMPro.TextMeshProUGUI cogSpawnedText;

    public TMPro.TextMeshPro m_SS1;
    public TMPro.TextMeshPro m_SS2;
    public TMPro.TextMeshPro m_SS3;
    public TMPro.TextMeshPro m_SS4;

    private Wave_System r_waveSystem;
    private Player_Controller r_playerController;
    private Prototype_Classes r_prototypeClasses;
    private Pickup_System pickupSystem;

    private float m_targetTime;
    private int m_currentSecond;
    private int m_currentMinute;

    private bool pauseMenuActive;

    public List<int> m_waveTimes = new List<int>();

    private void Start()
    {
        DontDestroyOnLoad(this);
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
        f_popupText();

        m_currentHealth.text = "" + r_playerController.playerHealth.ToString("F0");

        m_currentStoneCharge.text = "" + r_prototypeClasses.m_stonePower[r_prototypeClasses.m_classState].ToString("F0");

        m_SS1.text = r_prototypeClasses.m_stonePower[0].ToString("F0");
        m_SS2.text = r_prototypeClasses.m_stonePower[1].ToString("F0");
        m_SS3.text = r_prototypeClasses.m_stonePower[2].ToString("F0");
        m_SS4.text = r_prototypeClasses.m_stonePower[3].ToString("F0");

        if(r_waveSystem.enemiesLeft == 0 && r_waveSystem.curRound > 0)
        {
            chooseStone.enabled = true;
            timeTillNextRound.enabled = true;
        }
        else
        {
            chooseStone.enabled = false;
            timeTillNextRound.enabled = false;
        }

        timeTillNextRound.text = "NEXT ROUND IN " + r_waveSystem.m_currentIntermissionTime.ToString("F0");

        if (r_waveSystem.m_startedWaves == true)
        {
            m_currentSecond = Mathf.FloorToInt(m_targetTime % 60);
            m_currentMinute = Mathf.FloorToInt(m_targetTime / 60);

            m_targetTime -= Time.deltaTime;

            m_currentTimeText.text = "" + m_currentMinute.ToString("00") + ":" + m_currentSecond.ToString("00");

            if (m_targetTime <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuActive = !pauseMenuActive;
        }     

        pauseMenu.active = pauseMenuActive;
    }

    public void resumeButton()
    {
        pauseMenuActive = !pauseMenuActive;
    }

    public void exitButton()
    {
        gameUI.active = false;
        pauseMenuActive = false;
        SceneManager.LoadScene("MainMenu");      
    }

    void f_popupText()
    {
        if (pickupSystem.m_spawnNote == true)
        {
            noteSpawnedText.enabled = true;
            Invoke("f_resetText", 3);
        }
        if (pickupSystem.m_spawnCogs == true)
        {
            cogSpawnedText.enabled = true;
            Invoke("f_resetText", 3);
        }
    }

    void f_resetText()
    {
        noteSpawnedText.enabled = false;
        cogSpawnedText.enabled = false;
    }

    public void f_waveTimer()
    {
        m_targetTime = m_waveTimes[r_waveSystem.curRound];
    }
}
