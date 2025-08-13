using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;

    [NonSerialized] public bool LeftOrRight;
    private PlayerAttack playerattack;
    private UIManager uiManager;
    [NonSerialized] public GameObject pauseGO;

    void Awake()
    {
        pauseGO = FindAnyObjectByType<Pause>().gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerattack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiManager.Fading)
        {
            Idle();
            pauseGO.GetComponent<Pause>().enabled = false;
            return;
        }
        else
        {
            pauseGO.GetComponent<Pause>().enabled = true;
        }
        List<NPC> npc = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].talking)
            {
                Idle();
                return;
            }
        }

        if (playerattack.Attacking || GetComponent<PlayerHealth>().IsDead || pauseGO.GetComponent<Pause>().Paused) { return; }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!GetComponent<PlayerHealth>().CanRespawn) { return; }
            GetComponent<PlayerHealth>().Death();
        }

        //these lines get the input from from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //this line uses the input from the player on a bend tree to know what way to go. (-1 = left, 1 = right)
        if (movement.x != 0 || movement.y != 0)
        {
            if(movement.x != 0)
                anim.SetFloat("X_Movement", movement.x);
            anim.Play("Movement_Blend_Tree");

            //this stores the last animation (between left and right)
            if (movement.x == 1)
                LeftOrRight = true;
            if (movement.x == -1)
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
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length != 0)
        {
            if (LeftOrRight == true)
            {
                if (clipInfo[0].clip.name != "Warrior_Idle_0")
                    anim.Play("Warrior_Idle_0");
            }
            else
            {
                if (clipInfo[0].clip.name != "Warrior_Idle_Left_0")
                    anim.Play("Warrior_Idle_Left_0");
            }
        }
    }

    void FixedUpdate()
    {
        if (GetComponent<PlayerHealth>().IsDead || playerattack.Attacking) { return; }
        //this moves the player across the map
        rb.MovePosition(rb.position + movement.normalized * speed * Time.deltaTime); //this code moves the player to the new vector 2 position.
    }
}
