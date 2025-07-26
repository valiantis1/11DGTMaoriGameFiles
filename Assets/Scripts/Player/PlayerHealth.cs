using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int NumberHearts;
    [SerializeField] private GameObject HeartsPrefab;
    [SerializeField] private Transform HeartsLocation;
    [SerializeField] private List<GameObject> Hearts;
    private int CurrentHealth;
    private Animator anim;

    public bool CanTakeDamage = true;
    public bool IsDead;

    [SerializeField] private float WaitTime;
    [SerializeField] private int WaitLoops;

    void Awake()
    {
        anim = GetComponent<Animator>();

        //spawns in the hearts
        for (int i = 0; i < NumberHearts; i++)
        {
            Hearts.Add(Instantiate(HeartsPrefab));
            Hearts[i].transform.SetParent(HeartsLocation);
        }
        CurrentHealth = NumberHearts;
    }

    public void PlayerHit()
    {
        //this is here because i cant run the ienumerator from a different script
        StartCoroutine(TakeDamage());
    }

    private IEnumerator TakeDamage()
    {
        if(IsDead) { yield break; }
        CurrentHealth--;
        CanTakeDamage = false;

        //makes a heart dark
        Hearts[CurrentHealth].GetComponent<Image>().color = Color.gray1;

        if (CurrentHealth == 0)
        {
            Death();
            yield break;
        }

        //This makes the player flash red when taken damage.
        for (int i = 0; i < WaitLoops; i++)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(WaitTime / 2);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(WaitTime / 2);
        }

        CanTakeDamage = true;
    }

    private void GainHealth()
    {
        //gives the player health and sets a heart back to normal
        Hearts[CurrentHealth].GetComponent<Image>().color = Color.white;
        CurrentHealth++;
    }

    private void Death()
    {
        IsDead = true;
        anim.Play("Death");

        //Makes the Death animation in the right spot
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
    }

    public IEnumerator Delete()
    {
        for (int i = 0; i < NumberHearts; i++)
        {
            Destroy(Hearts[i]);
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }
}
