using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{

    // GORILLA

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Transform characterTransform;

    [SerializeField] private LayerMask jumpableGround;

    private bool isCrouched;
    private bool aimUp;
    private bool aimDown;

    private float dirX = 0f;
    private float dirY = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;

    PlayerHealth healthScript;


    private enum MovementState { idle, run, jump, fall, crouch, death, aim_up, aim_down }
    //                           0     1    2     3     4       5      6       7

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        characterTransform = GetComponent<Transform>();
        healthScript = transform.Find("Target").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!healthScript.isDead)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            dirY = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (IsGrounded())
            {
                if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    StandUp(); 
                }
            }

            


            if (Input.GetButtonDown("Fire3") & IsGrounded())
            {
                isCrouched = true;
            }
            else if (Input.GetButtonUp("Fire3"))
            {
                isCrouched = false;
            }


            if (Input.GetKeyDown(KeyCode.W))
            {
                aimUp = true;
                aimDown = false;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                aimUp = false;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                aimDown = true;
                aimUp = false;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                aimDown = false;
            }
            }

            if (Mathf.Abs(rb.velocity.x) > 0.01f)
            {
                StandUp();    
            }

            
            

            UpdateAnimationState();
        }
    }
    
    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.run;
            characterTransform.rotation = Quaternion.identity;
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            characterTransform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (isCrouched)
        {
            state = MovementState.crouch;
        }
        else if (aimUp)
        {
            state = MovementState.aim_up;
        }

        else if (aimDown)
        {
            state = MovementState.aim_down;
        }
        else
        {
            state = MovementState.idle;
        }

        

        



        if (rb.velocity.y > .1f & !IsGrounded())
        {
            state = MovementState.jump;

        }
        else if ((rb.velocity.y < -.1f & !isCrouched) & !IsGrounded())
        {
            state = MovementState.fall;
        }


        


        if (healthScript.isDead)
        {
            state = MovementState.death;
        }


        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0.1f, Vector2.down, 0.01f, jumpableGround);
    }

    private void StandUp()
    {
        isCrouched = false;
        aimUp = false;
        aimDown = false;
    }
}
