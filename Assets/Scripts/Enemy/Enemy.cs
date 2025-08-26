using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent; //(this is the AI)
    private Transform _playerTrans;

    private bool _leftOrRight;
    private Animator _anim;
    private bool _attacking;

    [SerializeField] private float _chaseRange, _leaveRange;

    public float Distance;
    public bool CanAttack;

    [SerializeField] private float range, hight, colliderDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D collider_;

    private GameObject _player;

    [NonSerialized] public bool IsDead;
    private bool _canGoToNewSpot = true;
    private bool _waitingToGo;
    private Vector3 _startPos;
    [SerializeField] private float randomWalkRange;

    void Awake()
    {
        //records the players start Position
        _startPos = transform.position;
        //Finds stuff in the scene
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        //settings
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        //fixs error with enemy running to player at start
        _agent.isStopped = true;
        StartCoroutine(FixStartMovement());
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if(_player == null)
                _player = FindAnyObjectByType<PlayerMovement>().gameObject;
        }
        catch { }
        
        //checks if the code should be run
        if (_agent.isStopped || _attacking || IsDead) { return; }

        if (_player != null) { _playerTrans = _player.transform; }

        //gets the last animations played
        AnimatorClipInfo[] clipInfo = _anim.GetCurrentAnimatorClipInfo(0);

        if (CanGoToPlayer())
            _agent.SetDestination(new Vector3(_playerTrans.position.x, _playerTrans.position.y, transform.position.z));
        else
            Looking();

        if (_agent.velocity.x != 0 || _agent.velocity.y != 0)
        {
            //there is only left and right animations 
            //this is here to basicly save what direction it moved last.
            if (_agent.velocity.x != 0)
                _anim.SetFloat("direction", _agent.velocity.normalized.x);
            _anim.Play("Movement_Blend_Tree"); //moving animations on

            if (_anim.GetFloat("direction") > 0) //left = false, right = true
                _leftOrRight = true;
            else
                _leftOrRight = false;
        }
        else
        {
            if (_leftOrRight == true)
            {
                if (clipInfo[0].clip.name != "Torch_Red_Idle")
                    _anim.Play("Torch_Red_Idle");
            }
            else
            {
                if (clipInfo[0].clip.name != "Torch_Red_Idle_Left")
                    _anim.Play("Torch_Red_Idle_Left");
            }
        }
    }

    private IEnumerator FixStartMovement()
    {
        yield return new WaitForSeconds(0.3f);
        _agent.isStopped = false;
    }

    private void Looking()
    {
        if(_agent.velocity.x == 0 && _agent.velocity.y == 0)
        {
            //waits 5 to 13 secounds then moves to a different random spot.
            if(!_waitingToGo)
            {
                StartCoroutine(NewSpotWait());
                _waitingToGo = true;
            }
            if(_canGoToNewSpot)
            {
                _agent.SetDestination(new Vector3(UnityEngine.Random.Range(_startPos.x - randomWalkRange, _startPos.x + randomWalkRange), UnityEngine.Random.Range(_startPos.y - randomWalkRange, _startPos.y + randomWalkRange), 0));
                _canGoToNewSpot = false;
                _waitingToGo = false;
            }
        }
    }

    private IEnumerator NewSpotWait()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(7, 15));
        _canGoToNewSpot = true;
    }

    private bool CanGoToPlayer()
    {
        if(_playerTrans == null) { return false; }
        Distance = Vector3.Distance(transform.position, _playerTrans.position);

        //checks if the player is in chase range
        //if the player is in the 'chase range' then the enemy is locked on to the player untill the player leaves the 'leave range'
        if (CanAttack)
        {
            if(Distance > _leaveRange)
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
            if(Distance > _chaseRange)
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
        if (_player.GetComponent<PlayerHealth>().IsDead || IsDead) { return; }
        StartAttack();
    }

    private void StartAttack()
    {
        if (IsDead) { return; }
        //gets the last animations played
        AnimatorClipInfo[] clipInfo = _anim.GetCurrentAnimatorClipInfo(0);

        //checks if attacking
        if (clipInfo[0].clip.name != "Torch_Red_Attack" || clipInfo[0].clip.name != "Torch_Red_Attack_Left")
            _attacking = false;
        
        //makes sure it is only ran once
        if (_attacking) {return;}

        _attacking = true;
        //stops movement
        _agent.SetDestination(transform.position);
        //attacks
        if (_leftOrRight)
            _anim.Play("Torch_Red_Attack");
        else
            _anim.Play("Torch_Red_Attack_Left");
    }

    public void Attack()
    {
        if (IsDead) { return; }
        //plays thoughout the animation and it not being run by code btw
        if (_leftOrRight)
        {
            //makes a Raycast hitbox to is see if there are anythings with the enemy layer
            RaycastHit2D hit =
              Physics2D.BoxCast(collider_.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z),
              0, Vector2.left, 0, playerLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
        else
        {
            //left side
            RaycastHit2D hit =
              Physics2D.BoxCast(collider_.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance,
              new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z),
              0, Vector2.left, 0, playerLayer);

            if (hit.collider != null)
            {
                HitEnemy(hit);
            }
        }
    }
    private void HitEnemy(RaycastHit2D hit)
    {
        if (!_player.GetComponent<PlayerHealth>().CanTakeDamage || IsDead) { return; }
        _player.GetComponent<PlayerHealth>().PlayerHit();
    }

    public void StopAttack()
    {
        if (IsDead) { return; }
        //players when the animation is finished (this is linked to the animation and it not being run by code)

        //lets the player attack again
        _attacking = false;
        //sets the movement back to the blend tree. So back to the normal moving (idling and running)
        _anim.Play("Movement_Blend_Tree");
    }

    private void OnDrawGizmos()
    {
        if (IsDead) { return; }
        Gizmos.color = Color.red;

        //draws a debug hitbox that can only been seen in the scene window

        //if the game is started or not
        if(_startPos != Vector3.zero)
            Gizmos.DrawWireSphere(_startPos, randomWalkRange);
        else
            Gizmos.DrawWireSphere(transform.position, randomWalkRange);


        Gizmos.DrawWireSphere(transform.position, _chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _leaveRange);
        Gizmos.color = Color.red;

        if (_leftOrRight)
            Gizmos.DrawWireCube(collider_.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z));
        else
            Gizmos.DrawWireCube(collider_.bounds.center + -transform.right * range * transform.localScale.x * colliderDistance, new Vector3(collider_.bounds.size.x * range, collider_.bounds.size.y + hight, collider_.bounds.size.z));
    }
}
