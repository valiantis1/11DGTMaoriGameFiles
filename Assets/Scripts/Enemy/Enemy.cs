using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent; //(this is the AI)
    private Transform PlayerTrans;

    private Vector3 direction;
    private bool LeftOrRight;
    private Animator anim;
    private bool Attacking;

    [SerializeField] private float AttackRange, LeaveRange;

    public float distance;
    public bool CanAttack;

    [SerializeField] private float range, Hight, colliderDistance;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private BoxCollider2D Collider;

    private GameObject Player;

    [SerializeField] private float WaitTimeRange1, WaitTimeRange2;

    [NonSerialized] public bool IsDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Player = FindAnyObjectByType<PlayerMovement>().gameObject;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;
        StartCoroutine(FixStartMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped || Attacking || IsDead) { return; }

        if (Player != null) { PlayerTrans = Player.transform; }

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (CanGoToPlayer())
            agent.SetDestination(new Vector3(PlayerTrans.position.x, PlayerTrans.position.y, transform.position.z));
        else
            Looking();

        if(PlayerTrans != null)
            direction = PlayerTrans.position - transform.position;

        if (agent.velocity.x != 0 && agent.velocity.y != 0)
        {
            if (agent.velocity.x != 0)
                anim.SetFloat("direction", direction.normalized.x);
            anim.Play("Movement_Blend_Tree");

            if (anim.GetFloat("direction") > 0)
                LeftOrRight = true;
            else
                LeftOrRight = false;
        }
        else
        {
            if (LeftOrRight == true)
            {
                if (clipInfo[0].clip.name != "Torch_Red_Idle")
                    anim.Play("Torch_Red_Idle");
            }
            else
            {
                if (clipInfo[0].clip.name != "Torch_Red_Idle_Left")
                    anim.Play("Torch_Red_Idle_Left");
            }
        }
    }

    private IEnumerator FixStartMovement()
    {
        yield return new WaitForSeconds(1);
        agent.isStopped = false;
    }

    private void Looking()
    {

    }

    private bool CanGoToPlayer()
    {
        if(PlayerTrans == null) { return false; }
        distance = Vector3.Distance(transform.position, PlayerTrans.position);

        if (CanAttack)
        {
            if(distance > LeaveRange)
            {
                CanAttack = false;
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if(distance > AttackRange)
            {
                return false;
            }
            else
            {
                CanAttack = true;
                return true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Player.GetComponent<PlayerHealth>().IsDead || IsDead) { return; }
        StartAttack();
    }

    private void StartAttack()
    {
        if (IsDead) { return; }
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo[0].clip.name != "Torch_Red_Attack" || clipInfo[0].clip.name != "Torch_Red_Attack_Left")
            Attacking = false;

        if (Attacking) {return;}

        Attacking = true;
        agent.SetDestination(transform.position);

        if (LeftOrRight)
            anim.Play("Torch_Red_Attack");
        else
            anim.Play("Torch_Red_Attack_Left");
    }

    public void Attack()
    {
        if (IsDead) { return; }
        //plays thoughout the animation and it not being run by code btw
        if (LeftOrRight)
        {
            //makes a Raycast hitbox to is see if there are anythings with the enemylayer
            RaycastHit2D hit =
              Physics2D.BoxCast(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z),
              0, Vector2.left, 0, PlayerLayer);

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
              0, Vector2.left, 0, PlayerLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
    }
    private void HitEnemy(RaycastHit2D hit)
    {
        if (!Player.GetComponent<PlayerHealth>().CanTakeDamage || IsDead) { return; }
        Player.GetComponent<PlayerHealth>().PlayerHit();
    }

    public void StopAttack()
    {
        if (IsDead) { return; }
        //players when the animation is finished (this is linked to the animation and it not being run by code)

        //lets the player attack again
        Attacking = false;
        //sets the movement back to the blend tree. So back to the normal moving (idling and running)
        anim.Play("Movement_Blend_Tree");
    }

    private void OnDrawGizmos()
    {
        if (IsDead) { return; }
        Gizmos.color = Color.red;

        //draws a debug hitbox that can only been seen in the scene window

        if (LeftOrRight)
            Gizmos.DrawWireCube(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
        else
            Gizmos.DrawWireCube(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
    }
}
