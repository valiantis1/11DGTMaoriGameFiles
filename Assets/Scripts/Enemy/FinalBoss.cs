using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3; // enemyPrefab1 will have 1 heart, enemyPrefab2 will have 2 hearts...
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private GameObject gates;

    [SerializeField] private GameObject tāwhirimātea_NPC_1; // this npc says "you are not a wero"
    [SerializeField] private GameObject tāwhirimātea_NPC_2; // this npc says the death speach
    [SerializeField] private GameObject narrator_NPC; // this npc says the death speach

    [SerializeField] private List<GameObject> enemys = new List<GameObject>();
    public bool IsFighting;

    private void OnTriggerEnter2D(Collider2D collision) // activated when player enters fighting zone
    {
        if (IsFighting) { return; }
        gates.SetActive(true);
        IsFighting = true;
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
        while (IsFighting)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                wave1 = true;
                wave2 = true;
                wave3 = true;
                wave4 = true;
                wave5 = true;
            }

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
                FindAnyObjectByType<Quests>().Trigger("death");
                IsFighting = false;
                yield break;
            }
            if (enemys.Count == 0)
            {
                if (!wave1)
                {
                    FindAnyObjectByType<Quests>().Trigger("BossFightStart");
                    SpawnEnemy(enemyPrefab1, 2);
                    wave1 = true;
                    yield return new WaitForSeconds(1);
                }
                else if (!wave2)
                {
                    SpawnEnemy(enemyPrefab1, 4);
                    wave2 = true;
                    yield return new WaitForSeconds(1);
                }
                else if (!wave3)
                {
                    tāwhirimātea_NPC_1.SetActive(true);
                    tāwhirimātea_NPC_1.GetComponent<NPC>().finalBossNpc();
                    yield return new WaitForSeconds(0.2f);
                    yield return new WaitUntil(DoneTalkingToNPC1);
                    SpawnEnemy(enemyPrefab2, 2);
                    wave3 = true;
                    yield return new WaitForSeconds(1);
                }
                else if (!wave4)
                {
                    SpawnEnemy(enemyPrefab2, 3);
                    wave4 = true;
                    yield return new WaitForSeconds(1);
                }
                else if (!wave5)
                {
                    SpawnEnemy(enemyPrefab3, 3);
                    wave5 = true;
                }
                else
                {
                    IsFighting = false; // stops the loop
                    tāwhirimātea_NPC_2.SetActive(true);
                    tāwhirimātea_NPC_2.GetComponent<NPC>().finalBossNpc(); // starts the talking with the NPC
                    yield return new WaitUntil(DoneTalkingToNPC2);

                    yield return new WaitForSeconds(2.5f);
                    narrator_NPC.SetActive(true);
                    narrator_NPC.GetComponent<NPC>().finalBossNpc(); // starts the narractor talk
                    yield return new WaitUntil(DoneTalkingToNPC3);

                    gates.SetActive(false); // Open gates when done
                    Application.Quit();
                }
            }

            yield return null;
        }
    }

    // I cant put all of the wait talking voids into one void because it doesnt work the same.
    private bool DoneTalkingToNPC1() //sees when the talking has stoped
    {
        if(tāwhirimātea_NPC_1.activeSelf)
        {
            if(!tāwhirimātea_NPC_1.GetComponent<NPC>().Talking) { return true; }
        }

        return false;
    }
    private bool DoneTalkingToNPC2() //sees when the talking has stoped
    {
        if (tāwhirimātea_NPC_2.activeSelf)
        {
            if (!tāwhirimātea_NPC_2.GetComponent<NPC>().Talking) { return true; }
        }

        return false;
    }
    
    private bool DoneTalkingToNPC3() // sees when the talking has stoped (this is the Narrator)
    {
        if (narrator_NPC.activeSelf)
        {
            if (!narrator_NPC.GetComponent<NPC>().Talking) { return true; }
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
                _amountOfEnemys -= 1; // limits the number of enemys to the desired amount
                GameObject enemy = default;
                enemy = Instantiate(_enemy, spawnPoints[i].transform.position - transform.position, Quaternion.identity); // spawns the enemy
                enemys.Add(enemy); // adds the enemy to a LIST
                enemy.SetActive(true);
                spawnPoints[i].GetComponent<FinalBossFightSpawnCheck>().CanSpawn = false; // doesnt let this code use the spawner again for a litte bit
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
