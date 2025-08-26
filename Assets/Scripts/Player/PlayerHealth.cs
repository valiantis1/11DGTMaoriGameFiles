using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int numberHearts;
    [SerializeField] private GameObject heartsPrefab;
    private Transform _heartsLocation;
    [SerializeField] private List<GameObject> hearts;
    private int _currentHealth;
    private Animator anim;

    public bool CanTakeDamage = true;
    public bool IsDead;
    private bool _startOfGame = true;

    [SerializeField] private float waitTime;
    [SerializeField] private int waitLoops;

    [NonSerialized] public bool CanRespawn;

    private UIManager uiManager;
    void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        _heartsLocation = FindAnyObjectByType<HorizontalLayoutGroup>().transform;
        anim = GetComponent<Animator>();

        StartCoroutine(MakeHearts());
    }

    public IEnumerator MakeHearts()
    {
        if (_startOfGame)
            yield return new WaitForSeconds(2.5f);
        _startOfGame = false;
        //spawns in the hearts
        for (int i = 0; i < numberHearts; i++)
        {
            hearts.Add(Instantiate(heartsPrefab));
            hearts[i].transform.SetParent(_heartsLocation);
            yield return new WaitForSeconds(0.2f);
        }
        _currentHealth = numberHearts;

        //turns on the movemnt for the first spawn in
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        CanRespawn = true;
    }

    public void PlayerHit()
    {
        if(!CanRespawn) { return; } // basically, spawn protection
        //this is here because i cant run the ienumerator from a different script
        StartCoroutine(TakeDamage());
    }

    private IEnumerator TakeDamage()
    {
        if(IsDead) { yield break; }
        _currentHealth--;
        CanTakeDamage = false;

        //makes a heart dark
        hearts[_currentHealth].GetComponent<Image>().color = Color.gray1;

        if (_currentHealth == 0)
        {
            Death();
            yield break;
        }

        //This makes the player flash red when taken damage.
        for (int i = 0; i < waitLoops; i++)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(waitTime / 2);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(waitTime / 2);
        }

        CanTakeDamage = true;
    }

    private void GainHealth()
    {
        //gives the player health and sets a heart back to normal
        hearts[_currentHealth].GetComponent<Image>().color = Color.white;
        _currentHealth++;
    }

    public void Death()
    {
        FindAnyObjectByType<NPC>().StopTalk = true;
        if(IsDead) { return; }
        IsDead = true;
        List<CapsuleCollider2D> capsuleCollider2D = GetComponents<CapsuleCollider2D>().ToList();
        for (int i = 0; i < capsuleCollider2D.Count; i++)
        {
            capsuleCollider2D[i].enabled = false;
        }
        GetComponent<SpriteRenderer>().sortingOrder = 8;
        anim.Play("Death");
        //Makes the Death animation in the right spot



        transform.position = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
    }

    public IEnumerator Delete()
    {
        for (int i = 0; i < numberHearts; i++)
        {
            Destroy(hearts[i]);
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
        FindAnyObjectByType<PlayerDeathManager>().death(); // tells the player death manager that the player is dead.
    }
}
