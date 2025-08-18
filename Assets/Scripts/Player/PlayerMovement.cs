using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Animator _anim;

    [NonSerialized] public bool LeftOrRight;
    private PlayerAttack _playerattack;
    private UIManager _uiManager;
    [NonSerialized] public GameObject PauseGO;

    void Awake()
    {
        PauseGO = FindAnyObjectByType<Pause>().gameObject;
        _uiManager = FindAnyObjectByType<UIManager>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _playerattack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_uiManager.Fading)
        {
            Idle();
            PauseGO.GetComponent<Pause>().enabled = false;
            return;
        }
        else
        {
            PauseGO.GetComponent<Pause>().enabled = true;
        }
        List<NPC> npc = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].Talking)
            {
                Idle();
                return;
            }
        }

        if (_playerattack.Attacking || GetComponent<PlayerHealth>().IsDead || PauseGO.GetComponent<Pause>().Paused) { return; }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!GetComponent<PlayerHealth>().CanRespawn) { return; }
            GetComponent<PlayerHealth>().Death();
        }

        //these lines get the input from from the player
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        //this line uses the input from the player on a bend tree to know what way to go. (-1 = left, 1 = right)
        if (_movement.x != 0 || _movement.y != 0)
        {
            if(_movement.x != 0)
                _anim.SetFloat("X_Movement", _movement.x);
            _anim.Play("Movement_Blend_Tree");

            //this stores the last animation (between left and right)
            if (_movement.x == 1)
                LeftOrRight = true;
            if (_movement.x == -1)
                LeftOrRight = false;
        }
        else
        {
            Idle();
        }
        
    }

    void Idle()
    {
        //gets all the current animations and stores
        AnimatorClipInfo[] clipInfo = _anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length != 0)
        {
            if (LeftOrRight == true)
            {
                if (clipInfo[0].clip.name != "Warrior_Idle_0")
                    _anim.Play("Warrior_Idle_0");
            }
            else
            {
                if (clipInfo[0].clip.name != "Warrior_Idle_Left_0")
                    _anim.Play("Warrior_Idle_Left_0");
            }
        }
    }

    void FixedUpdate()
    {
        List<NPC> npc = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].Talking)
            {
                return;
            }
        }

        if (GetComponent<PlayerHealth>().IsDead || _playerattack.Attacking) { return; }
        //this moves the player across the map
        _rb.MovePosition(_rb.position + _movement.normalized * speed * Time.deltaTime); //this code moves the player to the new vector 2 position.
    }
}
