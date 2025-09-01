using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float hight;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private CapsuleCollider2D collider_;
    [SerializeField] private PlayerMovement playermovement;
    [NonSerialized] public bool Attacking;
    private Animator _anim;
    private UIManager _uiManager;

    private bool _attackUpOrDown;

    [SerializeField] private AudioClip swordSwooshSound1, swordSwooshSound2, swordSwooshSound3;
    [SerializeField] private AudioSource audioSource;
    void Awake()
    {
        //finds it in the scene
        _uiManager = FindAnyObjectByType<UIManager>();
        _anim = GetComponent<Animator>();
    }
    void Update()
    {
        List<NPC> npc = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].Talking)
            {
                return;
            }
        }
        //checks if the player is dead.
        if (GetComponent<PlayerHealth>().IsDead || _uiManager.Fading || GetComponent<PlayerMovement>().PauseGO.GetComponent<Pause>().Paused) { return; }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            //When attacking, makes the player not be able to attack
            if (!Attacking)
            {
                StartAttacking();
                Attacking = true;
            }
        }
    }

    private void StartAttacking()
    {
        //makes a smoother attack and not as repetitive (with different up and down animations.)
        if (playermovement.LeftOrRight)
        {
            if(_attackUpOrDown)
                _anim.Play("Warrior_Attack1_0");
            else
                _anim.Play("Warrior_Attack2_0");
        }
        else
        {
            if(_attackUpOrDown)
                _anim.Play("Warrior_Attack1_Left_0");
            else
                _anim.Play("Warrior_Attack2_Left_0");
        }

        // plays a swoosh sound
        int oneToThree = 0;
        oneToThree = UnityEngine.Random.Range(1, 4);
        if(oneToThree == 1)
        {
            audioSource.clip = swordSwooshSound1;
        }
        if(oneToThree == 2)
        {
            audioSource.clip = swordSwooshSound2;
        }
        if (oneToThree == 3)
        {
            audioSource.clip = swordSwooshSound3;
        }

        // this makes the sound sound less repetitive by changing how it sounds
        audioSource.pitch = 0.8f + UnityEngine.Random.Range(0, 0.2f);
        audioSource.volume = 0.6f + UnityEngine.Random.Range(0,0.2f);
        audioSource.Play();
    }

    public void Attack()
    {
        //plays thoughout the animation and it not being run by code btw
        if (playermovement.LeftOrRight)
        {
            //makes a Raycast hitbox to is see if there are anythings with the enemy layer
            RaycastHit2D hit =
              Physics2D.BoxCast(collider_.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z),
              0, Vector2.left, 0, enemyLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
        else
        {
            //Left side check
            RaycastHit2D hit =
              Physics2D.BoxCast(collider_.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z),
              0, Vector2.left, 0, enemyLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
    }

    public void StopAttack()
    {
        //plays when the animation is finished (this is linked to the animation and it not being run by code)

        //lets the player attack again
        Attacking = false;
        //sets the movement back to the blend tree. So back to the normal moving (idling and running)
        _anim.Play("Movement_Blend_Tree");

        _attackUpOrDown = !_attackUpOrDown;
    }

    private void HitEnemy(RaycastHit2D hit)
    {
        hit.rigidbody.GetComponent<EnemyHealth>().Hit();
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        
        //draws a debug hitbox that can only been seen in the scene window

        if (playermovement.LeftOrRight)
            Gizmos.DrawWireCube(collider_.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z));
        else
            Gizmos.DrawWireCube(collider_.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z));
    }
}