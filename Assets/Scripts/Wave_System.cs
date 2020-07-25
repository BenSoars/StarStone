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

    private User_Interface m_Canvas;
    public AchievementTracker m_Achievement;
    public AchievementSpecialConditions m_SpecialTracker;
    private bool m_checkRound;

    //Kurtis Watson
    private Player_Controller r_playerController;
    private User_Interface r_userInterface;
    private Prototype_Classes r_prototypeClasses;
    private Pickup_System r_pickupSystem;

    public List<GameObject> m_wisps = new List<GameObject>();

    private GameObject[] m_wispPoint;
    private int m_random;

    public bool m_newWave;
    private bool m_enemiesKilled;

    private Text m_enemyCount;
    public float m_fogMath;
    private float m_spawnValue;

    public bool m_isIntermission;
    public bool notChosen;
    public float intermissionTime;
    public float m_currentIntermissionTime;

    //Kurtis Watson
    private void Start()
    {

        m_currentIntermissionTime = intermissionTime;
        // get the different components neccesart for this script to function
        m_wispPoint = GameObject.FindGameObjectsWithTag("WispPoint");
        r_playerController = FindObjectOfType<Player_Controller>();
        r_userInterface = FindObjectOfType<User_Interface>();
        r_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system
        r_pickupSystem = FindObjectOfType<Pickup_System>();
        m_Canvas = GameObject.Find("Canvas").GetComponent<User_Interface>();
        m_Achievement = GameObject.FindObjectOfType<AchievementTracker>();
        m_SpecialTracker = GameObject.FindObjectOfType<AchievementSpecialConditions>();
    }

    //Kurtis Watson
    private void Update()
    {
        f_updateUI();

        if (m_Canvas.runtimeUI.activeInHierarchy == true)
        {
            m_enemyCount = GameObject.Find("EnemyCount").GetComponent<Text>();
        }

        if (m_newWave == true)
        {
            r_userInterface.f_waveTimer();
            f_spawnWisps(); // spawn wisps
        }
    }

    //Kurtis Watson
    void f_spawnWisps()
    {
        if (SceneManager.GetActiveScene().name != "Temple_Clean") //Stop enemies spawning in clean scene.
        {
            m_newWave = false;
            m_currentIntermissionTime = intermissionTime; //Reset current intermission time.
             //Stop a new wave of enemies spawning. 
            m_startedWaves = true; //Update UI values.
            f_sortOutEnemys(); //Spawn enemies of different types.
            audio.playImportant(roundNoises[0]);
            for (int k = 0; k < enemyArray.Count; k++)
            {
                for (int i = 0; i < enemyArray[k]; i++)
                {
                    m_random = Random.Range(1, 4);
                    GameObject spawned = Instantiate(m_wisps[k], m_wispPoint[m_random].transform.position, Quaternion.identity); //Spawn wisps at a random point.
                    spawnedEnemies.Add(spawned); //Add enemy spawned to enemies spawned list.
                }
            }
            enemiesLeft = spawnedEnemies.Count; //Set the count for the enemies left.
            r_prototypeClasses.m_fogStrength = 0.2f; //Fog strength.
            r_prototypeClasses.m_currentFog = 0.2f; //Reset current fog.
            r_prototypeClasses.stonePower[r_prototypeClasses.chosenBuff] -= enemiesLeft * 2; //Decrease the chosen enemy buff by two times the amount of enemies.
            m_fogMath = r_prototypeClasses.m_fogStrength / enemiesLeft; //Calculate the amount of fog to decrease each enemy kill.
            curRound += 1; //Increase current round by one.
            m_enemiesKilled = false;
            

        }
    }

    //Kurtis Watson
    void f_updateUI()
    {
        if (m_Canvas.runtimeUI.activeInHierarchy == true && m_enemyCount)
        {
            m_enemyCount.text = ("" + enemiesLeft);
        }
    }

    //Kurtis Watson
    void FixedUpdate()
    {
        f_intermission();
        
        if (spawnedEnemies.Count <= 0 && enemiesLeft == 0 && m_enemiesKilled == false && curRound > 0)
        {
            r_prototypeClasses.buffChosen = false; //Bug fix.
            m_isIntermission = true; //Begin intermission countdown.
            if (m_spawnValue % 2 == 0) //Check for if the spawn value is a value of 2.
            {               
                r_pickupSystem.spawnNote = true; //Spawn note if value of 2.
            }
            else
            {
                r_pickupSystem.spawnCogs = true; //Spawn cogs if not a value of 2.
            }
            m_enemiesKilled = true; //Stops the pick-ups from spawning more than one item a round (Notes etc. above in %2 function).
            m_startedWaves = false; //Begin the wave (required for a different script).
            r_prototypeClasses.canSelect = true; //Allow the player to choose a new Starstone.
            r_prototypeClasses.activeStone[r_prototypeClasses.classState] = false; //Disable the current stone so that it can be chosen again.
            r_prototypeClasses.activeStone[r_prototypeClasses.chosenBuff] = false; //Disable the current enemy buff so it can be chosen again.
            m_spawnValue += 1; //Increments by one to indicate to the game on whether to spawn a gear cog or a lab note.
            r_userInterface.f_waveTimer(); //Update the round time limit.
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

    void f_intermission()
    {
        if (m_isIntermission == true) //Countdown for intermission between rounds.
        {
            if (m_checkRound == false)
            {
                m_SpecialTracker.CheckForRoundAchievements();
                m_checkRound = true;
            } 
            m_currentIntermissionTime -= Time.deltaTime;
        }
        else
        {
            m_checkRound = false;
        }

        if (m_currentIntermissionTime <= 0 && m_isIntermission == true) //If the player hasn't chosen a stone, the game will do it automatically.
        {
            notChosen = true; //Auto selection (used in Prototype_Classes).
            m_isIntermission = false; //Stops countdown as it has reached 0.
            r_prototypeClasses.canSelect = false; //Disable player selecting stone.
        }

        // Ben Soars

        //Achievement
        if (m_Achievement) // if there is an Achievement tracker in the scene
        {
            if (curRound > 1)
            {
                m_Achievement.UnlockAchievement(1); // unlock the beat round 1 Achievement
               
            }

            m_SpecialTracker.setRoundChecker();

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
