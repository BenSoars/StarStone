using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] // serializable class used for
public class Wave
{
    [Tooltip("The Wave Name")]
    public string name; // the name of the wave, set to wave (number) for clarity
    [Tooltip("Amount of Basic Type enemies per this wave")]
    public int Basic; // the amunt of basic enemies
    [Tooltip("Amount of Small Type enemies per this wave")]
    public int Small; // the amount of smalle enemies
    [Tooltip("Amount of Large Type enemies per this wave")]
    public int Large; // the amount of large enemies
    [Tooltip("Amount of Ranged Type enemies per this wave")]
    public int Ranged; // the amount or ranged enemies

    // all are separated for clarity, and making it easier for the producer to understand what each value is for
}


public class Wave_System : MonoBehaviour
{
    //Ben Soars
    [Header("Wave Components")]
    public List<GameObject> enemyTypes = new List<GameObject>(); // the enemy types
    //public List<string> amountOf = new List<string>(); // the amount of enemies per wave
    public List<GameObject> spawnedEnemies = new List<GameObject>(); // the spawned enemies
    public int enemiesLeft; // the amount of enemies remaining
    public int curRound; // the current round the player is on
    private List<int> m_enemyArray = new List<int>();
    public bool canSpawnEnemies;

    public Wave[] Waves;

    [Header("Audio System")]
    [Space(2)]
    public Audio_System audio; // get the audio system component to play sounds
    public List<AudioClip> roundNoises = new List<AudioClip>(); // the round noises

    [Header("Achievement System Components")]
    [Space(2)]
    private User_Interface m_canvas;
    public AchievementTracker m_Achievement;
    public AchievementSpecialConditions m_SpecialTracker;
    private bool m_checkRound;

    //Kurtis Watson     
    [Header("Wisp Components")]
    [Space(2)]
    public List<GameObject> wisps = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>(); //Stores the spawnpoints for the enemy.
    public bool m_startedWaves; //Detects if the wave has started.
    private GameObject[] m_wispPoint; //Spawn points.
    private int m_random; //Used to randomly choose where the enemy wisps should move towards.
    public bool newWave; //Check for new wave.
    private bool m_enemiesKilled; //Check for if all enemies have been killed.
    private Text m_enemyCount; //Displayed on screen.

    [Header("Script References")] //Required script references.
    [Space(2)]
    private User_Interface m_userInterface;
    private Prototype_Classes m_prototypeClasses;
    private Pickup_System m_pickupSystem;
    private Player_Controller m_playerContoller;

    [Header("Fog Values")]
    [Space(2)]
    [Tooltip("Calculate the fog intensity based on how many enemies are left.")]
    public float fogMath; //Used to work out how much fog to decrease by based on enemies.

    [Header("Intermission Components")]
    [Space(2)]
    public bool isIntermission; //Check for if intermission is live.
    public bool notChosen; //Check for if the player select a Starstone in time.
    [Tooltip("Set the time length of the intermission phases between rounds.")]
    public float intermissionTime; //Intermission time value.
    public float currentIntermissionTime; //Current intermission time.

    [Header("Drop Manager")]
    [Space(2)]
    private float m_spawnValue; //Detects whether to spawn a note or a clock part.

    //Kurtis Watson
    private void Start()
    {
        currentIntermissionTime = intermissionTime;

        canSpawnEnemies = true;

        m_wispPoint = GameObject.FindGameObjectsWithTag("WispPoint");
        m_userInterface = FindObjectOfType<User_Interface>();
        m_prototypeClasses = FindObjectOfType<Prototype_Classes>();
        audio = GameObject.FindObjectOfType<Audio_System>(); // get audio system
        m_pickupSystem = FindObjectOfType<Pickup_System>();
        m_canvas = GameObject.Find("Canvas").GetComponent<User_Interface>();
        m_Achievement = GameObject.FindObjectOfType<AchievementTracker>();
        m_SpecialTracker = GameObject.FindObjectOfType<AchievementSpecialConditions>();
        m_playerContoller = FindObjectOfType<Player_Controller>();

        for (int i = 1; i < 19; i++) //Add all the spawnpoints to a list.
        {
            spawnPoints.Add(GameObject.Find("SpawnPoint_" + i).transform);
        }
    }

    //Kurtis Watson
    private void Update()
    {
        f_updateUI();

        if (m_canvas.runtimeUI.activeInHierarchy == true)
        {
            m_enemyCount = GameObject.Find("EnemyCount").GetComponent<Text>();
        }

        if (newWave == true)
        {
            m_userInterface.f_waveTimer();
            f_spawnWisps(); // spawn wisps
        }
    }

    //Kurtis Watson
    void f_spawnWisps()
    {
        Debug.Log("f_spawnWisps function called.");
        
        newWave = false; //Stop this function being called again.
        currentIntermissionTime = intermissionTime; //Reset current intermission time.
        m_startedWaves = true; //Update UI values.

        f_sortOutEnemys(); //Spawn enemies of different types.
        audio.playImportant(roundNoises[0]);

        for (int k = 0; k < m_enemyArray.Count; k++)
        {
            for (int i = 0; i < m_enemyArray[k]; i++)
            {
                m_random = Random.Range(1, 4);
                GameObject spawned = Instantiate(wisps[k], m_wispPoint[m_random].transform.position, Quaternion.identity); //Spawn wisps at a random point.
                spawnedEnemies.Add(spawned); //Add enemy spawned to enemies spawned list.
            }
            Debug.Log("Total spawnedEnemies: " + spawnedEnemies.Count);
        }
        enemiesLeft = spawnedEnemies.Count; //Set the count for the enemies left.
        m_prototypeClasses.stonePower[m_prototypeClasses.chosenBuff] -= enemiesLeft * 2; //Decrease the chosen enemy buff by two times the amount of enemies.

        curRound += 1; //Increase current round by one.
        m_enemiesKilled = false;

        f_fogManager(); //Reset fog values.
        Debug.Log("New Wave: " + newWave);
    }

    //Kurtis Watson
    void f_fogManager()
    {
        m_prototypeClasses.fogStrength = 0.2f; //Fog strength.
        m_prototypeClasses.currentFog = 0.2f; //Reset current fog. 
        fogMath = m_prototypeClasses.fogStrength / enemiesLeft; //Calculate the amount of fog to decrease each enemy kill.
    }

    //Kurtis Watson
    void f_updateUI()
    {
        if (m_canvas.runtimeUI.activeInHierarchy == true && m_enemyCount)
        {
            m_enemyCount.text = ("" + enemiesLeft); //Display enemies left on the runtime UI.
        }
    }

    //Kurtis Watson
    void FixedUpdate()
    {
        f_intermission();

        if (spawnedEnemies.Count <= 0 && enemiesLeft == 0 && m_enemiesKilled == false && curRound > 0)
        {
            m_prototypeClasses.buffChosen = false; //Bug fix.
            isIntermission = true; //Begin intermission countdown.
            switch (curRound)
            {
                case 2:
                case 4:
                case 6:
                case 7:
                case 9:
                    m_pickupSystem.spawnCogs = true; //Spawn clock part if the case is met.
                    break;
                case 3:
                case 5:
                case 8:
                    m_pickupSystem.spawnNote = true; //Spawn note if the case is met.
                    break;
            }
            m_enemiesKilled = true; //Stops the pick-ups from spawning more than one item a round (Notes etc. above in %2 function).
            m_startedWaves = false; //Begin the wave (required for a different script).
            m_prototypeClasses.canSelect = true; //Allow the player to choose a new Starstone.
            m_prototypeClasses.activeStone[m_prototypeClasses.classState] = false; //Disable the current stone so that it can be chosen again.
            m_prototypeClasses.activeStone[m_prototypeClasses.chosenBuff] = false; //Disable the current enemy buff so it can be chosen again.
            m_spawnValue += 1; //Increments by one to indicate to the game on whether to spawn a gear cog or a lab note.
            m_userInterface.f_waveTimer(); //Update the round time limit.
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

    //Kurtis Watson
    void f_intermission()
    {
        if (isIntermission == true) //Countdown for intermission between rounds.
        {
            m_playerContoller.playerHealth = 100;
            if (m_checkRound == false)
            {
                m_SpecialTracker.CheckForRoundAchievements();
                m_checkRound = true;
            }
            currentIntermissionTime -= Time.deltaTime;
        }
        else
        {
            m_checkRound = false;
        }

        if (currentIntermissionTime <= 0 && isIntermission == true) //If the player hasn't chosen a stone, the game will do it automatically.
        {
            notChosen = true; //Auto selection (used in Prototype_Classes).
            isIntermission = false; //Stops countdown as it has reached 0.
            m_prototypeClasses.canSelect = false; //Disable player selecting stone.
        }


        // Ben Soars - Achievement
        if (m_Achievement) // if there is an Achievement tracker in the scene
        {
            if (curRound > 1)
            {
                m_Achievement.UnlockAchievement(1); // unlock the beat round 1 Achievement

            }
            m_SpecialTracker.setRoundChecker();
        }
    }

    void f_sortOutEnemys()
    {
        //Kurtis Watson
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Scene") && canSpawnEnemies == true)
        {
            canSpawnEnemies = false;
            curRound = 0;
        }
        //Ben Soars
        if (curRound <= Waves.Length) // if the current round isn't the last
        {
            m_enemyArray.Clear(); // clear the current array
            m_enemyArray.Add(Waves[curRound].Basic);
            m_enemyArray.Add(Waves[curRound].Small);
            m_enemyArray.Add(Waves[curRound].Large);
            m_enemyArray.Add(Waves[curRound].Ranged);
        }
        else
        {
            SceneManager.LoadScene("Game_Over"); // load 
        }
    }
}
