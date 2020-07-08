using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wave_System : MonoBehaviour
{
    // NEEDS TO BE COMMENTED 

    //Ben Soars
    public List<GameObject> enemyTypes = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();

    public List<string> amountOf = new List<string>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public int enemiesLeft;

    public int curRound;
    public List<int> enemyArray = new List<int>();
    public bool m_startedWaves;

    public Audio_System audio; // get the audio system component to play sounds
    public List<AudioClip> roundNoises = new List<AudioClip>();

    //Kurtis Watson
    private Player_Controller r_playerController;
    private User_Interface r_userInterface;
    private Prototype_Classes r_prototypeClasses;
    private Pickup_System r_pickupSystem;

    public List<GameObject> m_wisps = new List<GameObject>();

    private GameObject[] m_wispPoint;
    private int m_random;
    public int m_intermissionTime;

    public bool m_newWave;
    private bool m_timeMet;

    private Text m_enemyCount;
    public float m_fogMath;
    private float m_spawnValue;

    //Kurtis Watson
    private void Start()
    {
        m_wispPoint = GameObject.FindGameObjectsWithTag("WispPoint");
        r_playerController = FindObjectOfType<Player_Controller>();
        r_userInterface = FindObjectOfType<User_Interface>();
        r_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system
        r_pickupSystem = FindObjectOfType<Pickup_System>();
        m_enemyCount = GameObject.Find("EnemyCount").GetComponent<Text>();
    }

    //Kurtis Watson
    private void Update()
    {
        f_updateUI();

        if (m_newWave == true)
        {
            r_userInterface.f_waveTimer();
            StartCoroutine(f_spawnWisps()); // spawn wisps
        }
    }

    //Kurtis Watson
    IEnumerator f_spawnWisps()
    {
        m_newWave = false;
        
        yield return new WaitForSeconds(m_intermissionTime);
        m_startedWaves = true;
        f_sortOutEnemys();
        audio.playImportant(roundNoises[0]);
        for (int k = 0; k < enemyArray.Count; k++)
        {
            for (int i = 0; i < enemyArray[k]; i++)
            {
                m_random = Random.Range(0, 4);
                GameObject spawned = Instantiate(m_wisps[k], m_wispPoint[m_random].transform.position, Quaternion.identity);
                spawnedEnemies.Add(spawned);
            }
        }
        enemiesLeft = spawnedEnemies.Count;
        r_prototypeClasses.m_fogStrength = 0.2f; //THIS WILL NEED TO BE CHANGED IN ORDER TO ADD INTERMISSION BETWEEN ROUNDS.
        r_prototypeClasses.m_currentFog = 0.2f;
        r_prototypeClasses.m_stonePower[r_prototypeClasses.m_chosenBuff] -= enemiesLeft * 2;
        m_fogMath = r_prototypeClasses.m_fogStrength / enemiesLeft;
        curRound += 1;
        m_timeMet = false;
    }

    //Kurtis Watson
    void f_updateUI()
    {
        m_enemyCount.text = ("" + enemiesLeft);
    }

    // Update is called once per frame
    void FixedUpdate()
    {       
        //Ben Soars
        if (spawnedEnemies.Count <= 0 && enemiesLeft == 0 && m_timeMet == false)
        {
            m_timeMet = true;
            if (m_spawnValue % 2 == 0)
            {
                
                r_pickupSystem.m_spawnNote = true;
            }
            else
            {
                r_pickupSystem.m_spawnCogs = true;
            }
            m_startedWaves = false;
            r_prototypeClasses.m_canSelect = true;
            r_prototypeClasses.m_activeStone[r_prototypeClasses.m_classState] = false;            
            m_spawnValue += 1;
            r_userInterface.f_waveTimer();
            f_spawnWisps();
        }
        else
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies.RemoveAt(i);
                    break;
                }
            }
        }
    }

    //Ben Soars
    void f_sortOutEnemys()
    {
        if (curRound <= amountOf.Count)
        {
            string[] varArray = amountOf[curRound].Split('_');

            enemyArray.Clear();
            for (int i = 0; i < enemyTypes.Count; i++)
            {
                enemyArray.Add(System.Convert.ToInt32(varArray[i]));// convert the string into a string if it can
            }
        } else
        {
            SceneManager.LoadScene("MainMenu"); // load scene
        }
         
       
    }
}
