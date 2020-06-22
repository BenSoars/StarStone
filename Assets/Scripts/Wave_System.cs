using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave_System : MonoBehaviour
{
    //Ben Soars
    public List<GameObject> enemyTypes = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();

    public List<string> amountOf = new List<string>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public int enemiesLeft;

    public int curRound;
    public List<int> enemyArray = new List<int>();
    public bool m_startedWaves;

    //Kurtis Watson
    private Player_Controller r_playerController;
    private User_Interface r_userInterface;

    public List<GameObject> m_wisps = new List<GameObject>();

    private GameObject[] m_wispPoint;
    private int m_random;
    public bool m_startWaves;

    private Text m_enemyCount;


    //Kurtis Watson
    private void Start()
    {     
        m_wispPoint = GameObject.FindGameObjectsWithTag("WispPoint");
        r_playerController = FindObjectOfType<Player_Controller>();
        r_userInterface = FindObjectOfType<User_Interface>();
        m_enemyCount = GameObject.Find("EnemyCount").GetComponent<Text>();
    }

    //Kurtis Watson
    private void Update()
    {
        f_updateUI();
        if (m_startWaves == true)
        {
            m_startedWaves = true;
            r_userInterface.f_waveTimer();
            f_spawnWisps(); // spawn wisps
            m_startWaves = false;
        }
    }

    //Kurtis Watson
    void f_spawnWisps()
    {
        f_sortOutEnemys();
        for (int k = 0; k < enemyArray.Count; k++) { 
            for (int i = 0; i < enemyArray[k]; i++)
            {
                m_random = Random.Range(0, 4);
                GameObject spawned = Instantiate(m_wisps[k], m_wispPoint[m_random].transform.position, Quaternion.identity);
                spawnedEnemies.Add(spawned);
            }
        }
        enemiesLeft = spawnedEnemies.Count;
        curRound += 1;
        Debug.Log("Enemies Left: " + enemiesLeft);
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
        if (spawnedEnemies.Count <= 0 && enemiesLeft == 0)
        {
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
        string[] varArray = amountOf[curRound].Split('_');

        enemyArray.Clear();
        enemyArray.Add(System.Convert.ToInt32(varArray[0])); // convert the string into a string if it can
        enemyArray.Add(System.Convert.ToInt32(varArray[1]));
        enemyArray.Add(System.Convert.ToInt32(varArray[2]));
    }
}
