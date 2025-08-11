using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [NonSerialized] public float TimeAwayFromPlayer;
    private bool TouchingPlayer;
    [SerializeField] private int WhatTriggerAmI;

    private void Update()
    {
        if (!TouchingPlayer)
        {
            TimeAwayFromPlayer += Time.deltaTime;
        }
        try
        {
            FindAnyObjectByType<PlayerDeathManager>().RespawnTeller[WhatTriggerAmI] = gameObject; //tells the death manager
        }
        catch { }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        TimeAwayFromPlayer = 0;
        TouchingPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TouchingPlayer = false;
    }
}
