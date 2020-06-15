using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_System : MonoBehaviour
{
    public List<GameObject> enemyTypes = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();

    public List<int> amountOf = new List<int>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public int enemiesLeft;

    private int curRound;

    void SpawnEnemies()
    {
        for (int i = 0; i < amountOf[curRound]; i++)
        {
            int rando = Random.Range(0, spawnPoints.Count);
            GameObject spawned = Instantiate(enemyTypes[0], spawnPoints[rando].position, Quaternion.identity);
            spawnedEnemies.Add(spawned);
        }
        curRound += 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemiesLeft = spawnedEnemies.Count;

        if (spawnedEnemies.Count <= 0)
        {
            SpawnEnemies();
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
