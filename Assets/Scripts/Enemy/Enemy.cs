using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent; //(this is the AI)
    public Transform PlayerTrans;

    private Vector3 direction;
    private bool EnemyLeftOrRight;
    private Animator anim;
    private bool fighting;

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
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (!fighting)
            agent.SetDestination(new Vector3(PlayerTrans.position.x, PlayerTrans.position.y, transform.position.z));

        direction = PlayerTrans.position - transform.position;
        direction = direction.normalized;

        if (agent.velocity.x != 0 && agent.velocity.y != 0)
        {
            if (agent.velocity.x != 0)
                anim.SetFloat("direction", direction.x);
            anim.Play("Movement_Blend_Tree");

            if (anim.GetFloat("direction") > 0)
                EnemyLeftOrRight = true;
            else
                EnemyLeftOrRight = false;
        }
        else
        {
            if (EnemyLeftOrRight == true)
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
}
