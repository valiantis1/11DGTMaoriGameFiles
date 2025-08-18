using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _enemy;
    private bool _respawning;
    private bool _isPlayerClose;

    void Start()
    {
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemy == null && !_respawning)
        {
            StartCoroutine(RespawnEnemy());
            _respawning = true;
        }
    }

    private IEnumerator RespawnEnemy()
    {
        yield return new WaitUntil(CanSpawn);
        SpawnEnemy();
    }

    private bool CanSpawn()
    {
        if(!_isPlayerClose)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _isPlayerClose = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isPlayerClose = false;
    }

    private void SpawnEnemy()
    {
        _enemy = Instantiate(enemyPrefab);
        _enemy.transform.SetParent(gameObject.transform);

        _enemy.SetActive(false);
        _enemy.transform.position = transform.position;
        _enemy.SetActive(true);

        _respawning = false;
    }
}
