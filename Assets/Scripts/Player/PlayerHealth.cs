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

    public bool CanTakeDamage = true;
    public bool IsDead;

    [SerializeField] private float WaitTime;
    [SerializeField] private int WaitLoops;

    void Awake()
    {
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

        if (CurrentHealth == 0)
            Death();

        Hearts[CurrentHealth].GetComponent<Image>().color = Color.gray1;

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
        Hearts[CurrentHealth].GetComponent<Image>().color = Color.white;
        CurrentHealth++;
    }

    private void Death()
    {
        IsDead = true;
        print("Death");
    }
}
