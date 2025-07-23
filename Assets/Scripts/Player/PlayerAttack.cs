using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float Hight;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private CapsuleCollider2D Collider;
    [SerializeField] private PlayerMovement playermovement;
    [NonSerialized] public bool Attacking;
    private Animator anim;

    private bool AttackUpOrDown;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            //If not attacking makes the player not be able to attack when attacking
            if (!Attacking)
            {
                StartAttacking();
                Attacking = true;
            }
        }
    }

    private void StartAttacking()
    {
        //makes a smoother attack and not as repetitive
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
            //makes a Raycast hitbox to is see if there are anythings with the enemylayer
            RaycastHit2D hit =
              Physics2D.BoxCast(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z),
              0, Vector2.left, 0, EnemyLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
        else
        {
            RaycastHit2D hit =
              Physics2D.BoxCast(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z),
              0, Vector2.left, 0, EnemyLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
    }

    public void StopAttack()
    {
        //players when the animation is finished (this is linked to the animation and it not being run by code)

        //lets the player attack again
        Attacking = false;
        //sets the movement back to the blend tree. So back to the normal moving (idling and running)
        anim.Play("Movement_Blend_Tree");

        AttackUpOrDown = !AttackUpOrDown;
    }

    private void HitEnemy(RaycastHit2D hit)
    {
        print("hit");
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        
        //draws a debug hitbox that can only been seen in the scene window

        if (playermovement.LeftOrRight)
            Gizmos.DrawWireCube(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
        else
            Gizmos.DrawWireCube(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
    }
}