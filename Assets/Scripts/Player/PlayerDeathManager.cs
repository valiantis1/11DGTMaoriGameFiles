using System;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    private GameObject Player;

    void Awake()
    {
        Player = Instantiate(PlayerPrefab);
        Player.transform.SetParent(gameObject.transform);
    }

    public void death()
    {
        //plays little before when player is deleted

    }
}
