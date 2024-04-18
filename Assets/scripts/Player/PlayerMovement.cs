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
    public bool moveBool;
    private bool aimUp;
    private bool aimDown;
    [SerializeField] bool grounded;

    private float dirX = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;

    PlayerHealth healthScript;

    void OnEnable()
    {
        CameraManager.OnPlayerFastCamActive += HandleStartGame;
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        CameraManager.OnPlayerFastCamActive -= HandleStartGame;
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }

    private enum MovementState { idle, run, jump, fall, crouch, death, aim_up, aim_down }
    //                           0     1    2     3     4

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        characterTransform = GetComponent<Transform>();
        healthScript = transform.Find("Target").GetComponent<PlayerHealth>();

        UpdateAnimationState();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBool)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded())
                {
                    StandUp();
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) & IsGrounded())
            {
                isCrouched = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouched = false;
            }

            if (Input.GetKeyDown(KeyCode.W) & (dirX == 0))
            {
                aimUp = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                aimUp = false;
            }

            if (Input.GetKeyDown(KeyCode.S) & (dirX == 0))
            {
                aimDown = true;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                aimDown = false;
            }

            UpdateAnimationState();
        }

        grounded = IsGrounded();
    }
    
    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            StandUp();
            state = MovementState.run;
            characterTransform.rotation = Quaternion.identity;
        }
        else if (dirX < 0f)
        {
            StandUp();
            state = MovementState.run;
            characterTransform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (isCrouched)
        {
            state = MovementState.crouch;
        }
        else
        {
            state = MovementState.idle;
        }

        if (aimUp)
        {
            state = MovementState.aim_up;
        }

        else if (aimDown)
        {
            state = MovementState.aim_down;
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

    void HandleStartGame()
    {
        moveBool = true;
    }

    void HandlePlayerDeath()
    {
        moveBool = false;
    }
}
