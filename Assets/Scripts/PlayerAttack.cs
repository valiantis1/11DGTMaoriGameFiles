using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private CapsuleCollider2D Collider;
    [SerializeField] private PlayerMovement playermovement;
    [NonSerialized] public bool Attacking;
    private Animator anim;

    private bool AttackUpDownBool;
    [SerializeField] private float AttackTimeUpDown;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Attacking)
            {
                StartCoroutine(StartAttacking());
                Attacking = true;
            }
        }
    }

    private void Attack()
    {
        if (playermovement.LeftOrRight)
        {
            RaycastHit2D hit =
              Physics2D.BoxCast(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z),
              0, Vector2.left, 0, playerLayer);
        }
        else
        {
            RaycastHit2D hit =
              Physics2D.BoxCast(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z),
              0, Vector2.left, 0, playerLayer);
        }
    }

    private IEnumerator StartAttacking()
    {

        if(playermovement.LeftOrRight)
            anim.Play("Warrior_Attack1_0");
        else
            anim.Play("Warrior_Attack1_Left_0");

        //plays the attack multiple times to match the animation time.
        for (int i = 0; i < 4; i++)
        {
            Attack();
            yield return new WaitForSeconds(0.1f);
            print("attack"+i);
        }
        Attacking = false;
        anim.Play("Movement_Blend_Tree");

        AttackUpDownBool = true;
        StartCoroutine(AttackUpDown());
    }

    private IEnumerator AttackUpDown()
    {
        yield return new WaitForSeconds(AttackTimeUpDown);
        if (!Attacking)
            AttackUpDownBool = false;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        
        //draws a debug hitbox that can only been seen in the scene window

        if (playermovement.LeftOrRight)
            Gizmos.DrawWireCube(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z));
        else
            Gizmos.DrawWireCube(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z));
    }
}
