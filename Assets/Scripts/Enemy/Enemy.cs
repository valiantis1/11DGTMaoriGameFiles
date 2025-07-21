using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Vector3 target;
    NavMeshAgent agent; //(this is the AI)
    public Transform PlayerTrans;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(new Vector3(PlayerTrans.position.x, PlayerTrans.position.y, transform.position.z));
    }
}
