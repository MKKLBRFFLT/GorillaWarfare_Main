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

    private enum MovementState { idle, run, jump, fall, crouch }
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

        moveBool = false;
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
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }

            if (Input.GetButtonDown("Fire1") & IsGrounded())
            {
                isCrouched = true;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                isCrouched = false;
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
        else if (isCrouched & IsGrounded())
        {
            state = MovementState.crouch;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jump;

        }
        else if (rb.velocity.y < -.1f & !isCrouched)
        {
            state = MovementState.fall;
        }


        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.4f, jumpableGround);
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
