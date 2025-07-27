using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent; //(this is the AI)
    private Transform PlayerTrans;

    private bool LeftOrRight;
    private Animator anim;
    private bool Attacking;

    [SerializeField] private float ChaseRange, LeaveRange;

    public float distance;
    public bool CanAttack;

    [SerializeField] private float range, Hight, colliderDistance;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private BoxCollider2D Collider;

    private GameObject Player;

    [NonSerialized] public bool IsDead;
    private bool CanGoToNewSpot = true;
    private bool WaitingToGo;
    private Vector3 StartPos;
    [SerializeField] private float RandomWalkRange;

    void Awake()
    {
        //records the players start Position
        StartPos = transform.position;
        //Finds stuff in the scene
        Player = FindAnyObjectByType<PlayerMovement>().gameObject;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //settings
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //fixs error with enemy running to player at start
        agent.isStopped = true;
        StartCoroutine(FixStartMovement());
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the code should be run
        if (agent.isStopped || Attacking || IsDead) { return; }

        if (Player != null) { PlayerTrans = Player.transform; }

        //gets the last animations played
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (CanGoToPlayer())
            agent.SetDestination(new Vector3(PlayerTrans.position.x, PlayerTrans.position.y, transform.position.z));
        else
            Looking();

        if (agent.velocity.x != 0 || agent.velocity.y != 0)
        {
            //there is only left and right animations 
            //this is here to basicly save what direction it moved last.
            if (agent.velocity.x != 0)
                anim.SetFloat("direction", agent.velocity.normalized.x);
            anim.Play("Movement_Blend_Tree"); //moving animations on

            if (anim.GetFloat("direction") > 0) //left = false, right = true
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
        if(agent.velocity.x == 0 && agent.velocity.y == 0)
        {
            //waits 5 to 13 secounds then moves to a different random spot.
            if(!WaitingToGo)
            {
                StartCoroutine(NewSpotWait());
                WaitingToGo = true;
            }
            if(CanGoToNewSpot)
            {
                agent.SetDestination(new Vector3(UnityEngine.Random.Range(StartPos.x - RandomWalkRange, StartPos.x + RandomWalkRange), UnityEngine.Random.Range(StartPos.y - RandomWalkRange, StartPos.y + RandomWalkRange), 0));
                CanGoToNewSpot = false;
                WaitingToGo = false;
            }
        }
    }

    private IEnumerator NewSpotWait()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(5, 13));
        CanGoToNewSpot = true;
    }

    private bool CanGoToPlayer()
    {
        if(PlayerTrans == null) { return false; }
        distance = Vector3.Distance(transform.position, PlayerTrans.position);

        //checks if the player is in chase range
        //if the player is in the 'chase range' then the enemy is locked on to the player untill the player leaves the 'leave range'
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
            if(distance > ChaseRange)
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
        //This code is on when the player has entered a hitbox (the attack range)
        if (Player.GetComponent<PlayerHealth>().IsDead || IsDead) { return; }
        StartAttack();
    }

    private void StartAttack()
    {
        if (IsDead) { return; }
        //gets the last animations played
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        //checks if attacking
        if (clipInfo[0].clip.name != "Torch_Red_Attack" || clipInfo[0].clip.name != "Torch_Red_Attack_Left")
            Attacking = false;
        
        //makes sure it is only ran once
        if (Attacking) {return;}

        Attacking = true;
        //stops movement
        agent.SetDestination(transform.position);
        //attacks
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
            //makes a Raycast hitbox to is see if there are anythings with the enemy layer
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
            //left side
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

        //if the game is started or not
        if(StartPos != Vector3.zero)
            Gizmos.DrawWireSphere(StartPos, RandomWalkRange);
        else
            Gizmos.DrawWireSphere(transform.position, RandomWalkRange);


        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LeaveRange);
        Gizmos.color = Color.red;

        if (LeftOrRight)
            Gizmos.DrawWireCube(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
        else
            Gizmos.DrawWireCube(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
    }
}
