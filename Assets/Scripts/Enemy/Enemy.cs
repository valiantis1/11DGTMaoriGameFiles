using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent; //(this is the AI)
    [SerializeField] private Transform PlayerTrans;

    private Vector3 direction;
    private bool LeftOrRight;
    private Animator anim;
    private bool Attacking;

    [SerializeField] private float AttackRange;
    [SerializeField] private float LeaveRange;
    public float distance;
    public bool CanAttack;

    [SerializeField] private float range;
    [SerializeField] private float Hight;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private BoxCollider2D Collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Attacking) { return; }

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if(CanGoToPlayer())
            agent.SetDestination(new Vector3(PlayerTrans.position.x, PlayerTrans.position.y, transform.position.z));

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

    private bool CanGoToPlayer()
    {
        float NewDirectionX;
        float NewDirectionY;

        if (direction.x < 0)
            NewDirectionX = direction.x * -1;
        else
            NewDirectionX = direction.x;

        if (direction.y < 0)
            NewDirectionY = direction.y * -1;
        else
            NewDirectionY = direction.y;

        distance = NewDirectionX + NewDirectionY;

        if(CanAttack)
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
        StartAttack();
    }

    private void StartAttack()
    {
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
        print("Enemy hit");
    }

    public void StopAttack()
    {
        //players when the animation is finished (this is linked to the animation and it not being run by code)

        //lets the player attack again
        Attacking = false;
        //sets the movement back to the blend tree. So back to the normal moving (idling and running)
        anim.Play("Movement_Blend_Tree");
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        //draws a debug hitbox that can only been seen in the scene window

        if (LeftOrRight)
            Gizmos.DrawWireCube(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
        else
            Gizmos.DrawWireCube(Collider.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y + Hight, Collider.bounds.size.z));
    }
}
