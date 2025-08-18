using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [NonSerialized] public float TimeAwayFromPlayer;
    private bool _touchingPlayer;
    [SerializeField] private int whatTriggerAmI;

    private void Update()
    {
        if (!_touchingPlayer)
        {
            TimeAwayFromPlayer += Time.deltaTime;
        }
        try
        {
            FindAnyObjectByType<PlayerDeathManager>().RespawnTeller[whatTriggerAmI] = gameObject; //tells the death manager
        }
        catch { }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        TimeAwayFromPlayer = 0;
        _touchingPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _touchingPlayer = false;
    }
}
