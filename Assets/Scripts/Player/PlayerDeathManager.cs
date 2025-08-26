using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    private GameObject _player;

    private List<Vector2> RespawnLocations = new List<Vector2> 
    { new Vector2(-2.5f, 1), // start position
      new Vector2(-9, 12), // start town
      new Vector2(7, 40), // near 1st island fight
      new Vector2(25, 44), // entrance 2nd island
      new Vector2(61.5f, 41), // 2nd island
      new Vector2(64, 15.7f), // start of long walk to 3rd island
      new Vector2(69, 80), // entrance 3rd island
      new Vector2(55, 111), // town near boss fight
      new Vector2(101.6f, 96.76f)}; // boss fight

    [NonSerialized] public List<GameObject> RespawnTeller = new List<GameObject> { null, null, null, null, null, null, null, null, null };

    void Awake()
    {
        _player = Instantiate(PlayerPrefab);
        _player.transform.SetParent(gameObject.transform);
    }
    public void death()
    {
        //Fades the screen
        FindAnyObjectByType<UIManager>().PlayerDeath();
    }
    public void SpawnPlayer()
    {
        _player = Instantiate(PlayerPrefab);
        _player.transform.SetParent(gameObject.transform);
        _player.transform.localPosition = RespawnPoint();
    }

    public Vector3 RespawnPoint()
    {
        GameObject ClosestSpawnPoint = null;
        int Point = 0;
        for (int i = 0; i < RespawnTeller.Count; i++)
        {
            try
            {
                if (ClosestSpawnPoint == null || RespawnTeller[i].GetComponent<Respawn>().TimeAwayFromPlayer < ClosestSpawnPoint.GetComponent<Respawn>().TimeAwayFromPlayer) { ClosestSpawnPoint = RespawnTeller[i]; Point = i; }
            }
            catch { }
        }
        return RespawnLocations[Point];
    }
}
