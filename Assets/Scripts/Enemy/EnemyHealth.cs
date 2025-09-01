using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startHealth;
    private int _currentHealth;

    private SpriteRenderer _sprite;
    private Animator _anim;

    private bool _canBeAttacked = true;
    private float _waitingTime = 0.5f;

    [SerializeField] private AudioSource audioSource; // for the fire effects

    private void Start()
    {
        //finds everything in the scene
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        //makes sure the health isnt set to high
        if (startHealth > 3)
            startHealth = 3;
        _currentHealth = startHealth;
    }

    public void Hit()
    {
        //this gets call when the player is hit
        if (!_canBeAttacked) { return; } //makes sure the player cant be hit or than once per swing
        _currentHealth--;
        if(Dead())
        {
            audioSource.Stop();
            //sets colour back to normal, plays animation and tells the Enemy script to stop everything
            _sprite.color = Color.white;
            GetComponent<Enemy>().IsDead = true;
            _anim.Play("Death");

            //stops the player from moving by turning off the sript
            GetComponent<Enemy>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            //The enemy has 2 box Colliders
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.6f, gameObject.transform.position.z);
        }
        else
        {
            //works out a nice colour to make the enemy go red.
            float DecreaseAmount;
            DecreaseAmount = math.round(50 * _currentHealth / startHealth);
            DecreaseAmount *= 0.01f;
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g - DecreaseAmount, _sprite.color.b - DecreaseAmount);
        }
        StartCoroutine(CheckCanBeAttacked());
    }

    private bool Dead()
    {
        if (_currentHealth >= 1)
        {
            return false;
        }
        return true;
    }

    private IEnumerator CheckCanBeAttacked()
    {
        _canBeAttacked = false;
        yield return new WaitForSeconds(_waitingTime);
        _canBeAttacked = true;
    }

    public void DeleteSelf()
    {
        FindAnyObjectByType<Quests>().EnemysDefeated();
        Destroy(gameObject);
    }
}
