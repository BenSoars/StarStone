using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave_System : MonoBehaviour
{
    //Ben Soars
    public List<GameObject> enemyTypes = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();

    public List<int> amountOf = new List<int>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public int enemiesLeft;

    private int curRound;

    //Kurtis Watson
    private Player_Controller r_playerController;
    public GameObject m_wisp;
    private GameObject[] m_wispPoint;
    private int m_random;
    public bool m_startWaves;

    private Text m_enemyCount;


    //Kurtis Watson
    private void Start()
    {     
        m_wispPoint = GameObject.FindGameObjectsWithTag("WispPoint");
        r_playerController = FindObjectOfType<Player_Controller>();

        m_enemyCount = GameObject.Find("EnemyCount").GetComponent<Text>();
    }

    //Kurtis Watson
    private void Update()
    {
        f_updateUI();
        if (m_startWaves == true)
        {
            f_spawnWisps();
            m_startWaves = false;
        }
    }

    //Kurtis Watson
    void f_spawnWisps()
    {     
        for (int i = 0; i < amountOf[curRound]; i++)
        {
            m_random = Random.Range(0, 4);          
            GameObject spawned = Instantiate(m_wisp, m_wispPoint[m_random].transform.position, Quaternion.identity);
            spawnedEnemies.Add(spawned);
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
}
