using System.Collections;
using UnityEngine;

public class FinalBossFightSpawnCheck : MonoBehaviour
{
    public bool CanSpawn = true;
    private bool _waiting;
    private void Update()
    {
        if(!CanSpawn && !_waiting)
        {
            StartCoroutine(Wait());
            _waiting = true;
        }
    }

    private IEnumerator Wait() // can spawn again after 1 to 5 seconds
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
        CanSpawn = true;
        _waiting = false;
    }
}
