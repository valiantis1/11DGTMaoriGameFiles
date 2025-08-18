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
    private Animator anim;
    private UIManager uiManager;

    private bool AttackUpOrDown;

    void Awake()
    {
        //finds it in the scene
        uiManager = FindAnyObjectByType<UIManager>();
        anim = GetComponent<Animator>();
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
        if (GetComponent<PlayerHealth>().IsDead || uiManager.Fading || GetComponent<PlayerMovement>().PauseGO.GetComponent<Pause>().Paused) { return; }
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
            if(AttackUpOrDown)
                anim.Play("Warrior_Attack1_0");
            else
                anim.Play("Warrior_Attack2_0");
        }
        else
        {
            if(AttackUpOrDown)
                anim.Play("Warrior_Attack1_Left_0");
            else
                anim.Play("Warrior_Attack2_Left_0");
        }
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
        anim.Play("Movement_Blend_Tree");

        AttackUpOrDown = !AttackUpOrDown;
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