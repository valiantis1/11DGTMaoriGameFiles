using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3; // 1 will be the weakest with 3 being the strongest
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private GameObject gates;

    [SerializeField] private List<GameObject> tāwhirimātea_NPCs;

    [SerializeField] private List<GameObject> enemys = new List<GameObject>();
    private bool _isFighting;

    private void OnTriggerEnter2D(Collider2D collision) // activated when player enters fighting zone
    {
        if (_isFighting) { return; }
        gates.SetActive(true);
        _isFighting = true;
        StartCoroutine(Fight());
    }

    private IEnumerator Fight()
    {
        // sets waves
        bool wave1 = false;
        bool wave2 = false;
        bool wave3 = false;
        bool wave4 = false;
        bool wave5 = false;
        while (_isFighting)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i] == null) // if an enemy is dead, the enemy will be removed from the list
                {
                    enemys.RemoveAt(i);
                }
            }
            // Check if player is dead
            if (FindAnyObjectByType<PlayerMovement>() == null)
            {
                //print("Player die :(");
                StartCoroutine(Death());
                _isFighting = false;
                yield break;
            }
            if (enemys.Count == 0)
            {
                if (!wave1)
                {
                    SpawnEnemy(enemyPrefab1, 2);
                    wave1 = true;
                    //print("Wave1");
                    yield return new WaitForSeconds(1f); // Brief delay
                }
                else if (!wave2)
                {
                    SpawnEnemy(enemyPrefab1, 4);
                    wave2 = true;
                    //print("Wave2");
                    yield return new WaitForSeconds(1f);
                }
                else if (!wave3)
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return new WaitUntil(DoneTalking);
                    SpawnEnemy(enemyPrefab2, 2);
                    wave3 = true;
                    //print("Wave3");
                    yield return new WaitForSeconds(1f);
                }
                else if (!wave4)
                {
                    SpawnEnemy(enemyPrefab2, 3);
                    wave4 = true;
                    //print("Wave4");
                    yield return new WaitForSeconds(1f);
                }
                else if (!wave5)
                {
                    SpawnEnemy(enemyPrefab3, 3);
                    wave5 = true;
                    //print("Wave5");
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    _isFighting = false;
                    print("All waves completed!");
                    gates.SetActive(false); // Open gates when done
                }
            }

            yield return null;
        }
    }

    private bool DoneTalking()
    {
        for (int i = 0; tāwhirimātea_NPCs.Count < 5; i++)
        {

        }

        return false;
    }

    private void SpawnEnemy(GameObject _enemy, int _amountOfEnemys) //spawns the enemy at a random spawnpoint
    {
        while(_amountOfEnemys > 0)
        {
            int i = UnityEngine.Random.Range(0, spawnPoints.Count);
            if (spawnPoints[i].GetComponent<FinalBossFightSpawnCheck>().CanSpawn)
            {
                _amountOfEnemys -= 1;
                GameObject enemy = default;
                enemy = Instantiate(_enemy, spawnPoints[i].transform.position - transform.position, Quaternion.identity);
                enemys.Add(enemy);
                enemy.SetActive(true);
                spawnPoints[i].GetComponent<FinalBossFightSpawnCheck>().CanSpawn = false;
            }
        }
    }

    private IEnumerator Death() // activated when playes dies
    {
        yield return new WaitForSeconds(2); // waits for the screen to fade
        gates.SetActive(false);
        for (int i = 0; i < enemys.Count; i++)
        {
            Destroy(enemys[i]);
        }
        enemys.Clear();
    }

}
