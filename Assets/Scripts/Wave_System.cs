using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wave_System : MonoBehaviour
{


    //Ben Soars
    public List<GameObject> enemyTypes = new List<GameObject>(); // the enemy types
    public List<Transform> spawnPoints = new List<Transform>(); // the spawn points for the enemies

    public List<string> amountOf = new List<string>(); // the amount of enemies per wave
    public List<GameObject> spawnedEnemies = new List<GameObject>(); // the spawned enemies
    public int enemiesLeft; // the amount of enemies remaining

    public int curRound; // the current round the player is on
    public List<int> enemyArray = new List<int>(); 
    public bool m_startedWaves; // used to check if the wave has been started

    public Audio_System audio; // get the audio system component to play sounds
    public List<AudioClip> roundNoises = new List<AudioClip>(); // the round noises

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
        // get the different components neccesart for this script to function
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
        //Kurtis Watson
        if (spawnedEnemies.Count <= 0 && enemiesLeft == 0 && m_timeMet == false) // if there are no more enemies
        {
            m_timeMet = true;
            if (m_spawnValue % 2 == 0)// if the spawn value is a multiple of two
            {
                
                r_pickupSystem.m_spawnNote = true; // spawn a note
            }
            else
            {
                r_pickupSystem.m_spawnCogs = true; // spawn a cog
            }
            m_startedWaves = false; // started the wave
            r_prototypeClasses.m_canSelect = true;
            r_prototypeClasses.m_activeStone[r_prototypeClasses.m_classState] = false;   // select a class for the prototype weapon          
            m_spawnValue += 1; // increase spawn value
            r_userInterface.f_waveTimer(); // update the wave timer
            f_spawnWisps(); // run wisp spawn function
        }
        else
        {
            //Ben Soars
            // if there are enemies
            for (int i = 0; i < spawnedEnemies.Count; i++) //check the list of enemies that are spawned
            {
                if (spawnedEnemies[i] == null) // if the enemy at that point doesn't exsist
                {
                    spawnedEnemies.RemoveAt(i); // remove eveny from the list
                    break; // break the loop, as the for loop won't work due to an element being removed
                }
            }
        }
    }

    //Ben Soars
    void f_sortOutEnemys()
    {
        if (curRound <= amountOf.Count) // if the current round isn't the last
        {
            string[] varArray = amountOf[curRound].Split('_'); // split the current amount of enemies

            enemyArray.Clear(); // clear the current array
            for (int i = 0; i < enemyTypes.Count; i++) // for loop for all the enemy types
            {
                enemyArray.Add(System.Convert.ToInt32(varArray[i]));// convert the string into a string if it can
            }
        } else
        {
            SceneManager.LoadScene("MainMenu"); // load 
        }
         
       
    }
}
