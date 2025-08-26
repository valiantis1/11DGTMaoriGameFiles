using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour //this script is made to fix a problem I had with have multiple NPCs.
{
    [SerializeField] List<NPC> NPCs = new List<NPC>();
    private PlayerDeathManager _playerDeathManager;
    private Vector3 _playerPos;
    private bool _isHeartMade; // it is called this because when the hearts are first made PlayerMovement gets turned on

    private void Awake()
    {
        _playerDeathManager = FindAnyObjectByType<PlayerDeathManager>();
    }

    private void Update()
    {
        if (_isHeartMade) { return; }
        if(FindAnyObjectByType<PlayerMovement>().enabled && !_isHeartMade)
        {
            _isHeartMade = true;
            StartCoroutine(Loop());
        }
    }

    private IEnumerator Loop() // this code plays every 1 second to stop lag
    {
        // this code finds the closest NPC then only that NPC will be on and other NPC's will be off


        int npc = -1;
        try
        {
            _playerPos = FindAnyObjectByType<PlayerMovement>().gameObject.transform.position;

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < NPCs.Count; i++)
            {
                float distance = Vector3.Distance(NPCs[i].transform.position, _playerPos); //gets the distance between the two points and changes the vector 3 to a float

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    npc = i;
                }
            }
            for (int i = 0; i < NPCs.Count; i++) //shows the closest NPC and hides the other NPC's
            {
                if (i != npc)
                {
                    NPCs[i].gameObject.SetActive(false);
                }
                else if(npc != -1)
                {
                    NPCs[npc].gameObject.SetActive(true);
                }
            }

        } catch { }
        npc = -1;

        yield return new WaitForSeconds(1f);
        StartCoroutine(Loop());
    }
}
