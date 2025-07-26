using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int StartHealth;
    private int CurrentHealth;

    private SpriteRenderer Sprite;
    private Animator anim;

    private bool CanBeAttacked = true;
    private float WaitingTime = 0.5f;

    private void Start()
    {
        //finds everything in the scene
        anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        //makes sure the health isnt set to high
        if (StartHealth > 3)
            StartHealth = 3;
        CurrentHealth = StartHealth;
    }

    public void Hit()
    {
        //this gets call when the player is hit
        if (!CanBeAttacked) { return; } //makes sure the player cant be hit or than once per swing
        CurrentHealth--;
        if(Dead())
        {
            //sets colour back to normal, plays animation and tells the Enemy script to stop everything
            Sprite.color = Color.white;
            GetComponent<Enemy>().IsDead = true;
            anim.Play("Death");

            //stops the player from moving by turning off the sript
            GetComponent<Enemy>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            //The enemy has 2 box Colliders
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            //works out a nice colour to make the enemy go red.
            float DecreaseAmount;
            DecreaseAmount = math.round(50 * CurrentHealth / StartHealth);
            DecreaseAmount *= 0.01f;
            Sprite.color = new Color(Sprite.color.r, Sprite.color.g - DecreaseAmount, Sprite.color.b - DecreaseAmount);
        }
        StartCoroutine(CheckCanBeAttacked());
    }

    private bool Dead()
    {
        if (CurrentHealth >= 1)
        {
            return false;
        }
        return true;
    }

    private IEnumerator CheckCanBeAttacked()
    {
        CanBeAttacked = false;
        yield return new WaitForSeconds(WaitingTime);
        CanBeAttacked = true;
    }

    public void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
