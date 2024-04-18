using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{

    // GORILLA

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public Animator anim;
    private Transform characterTransform;

    [SerializeField] private LayerMask jumpableGround;

    private bool isCrouched;
    public bool moveBool;
    private bool aimUp;
    private bool aimDown;
    bool jumping;
    bool running;

    private float dirX = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] AudioClip jumpStart2;
    [SerializeField] AudioClip jumpLand2;
    [SerializeField] AudioClip run1;
    [SerializeField] AudioClip run2;
    [SerializeField] AudioClip run3;
    [SerializeField] AudioClip run4;
    [SerializeField] AudioClip run5;
    [SerializeField] AudioClip run6;
    [SerializeField] AudioClip run7;
    [SerializeField] AudioClip run8;
    [SerializeField] AudioClip run9;
    [SerializeField] AudioClip run10;

    PlayerHealth healthScript;
    AudioManager audioManager;

    void OnEnable()
    {
        CameraManager.OnPlayerFastCamActive += HandleStartGame;
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
        Finish.OnLevelComplete += HandleFinishGame;
    }

    void OnDisable()
    {
        CameraManager.OnPlayerFastCamActive -= HandleStartGame;
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
        Finish.OnLevelComplete -= HandleFinishGame;
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
        audioManager = GameObject.FindWithTag("Managers").GetComponent<AudioManager>();

        UpdateAnimationState();

        Time.timeScale = 1f;
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
                    audioManager.PlayClip(jumpStart2, "sfx");
                    StartCoroutine(StartJump());
                }
            }

            if (jumping && IsGrounded())
            {
                audioManager.PlayClip(jumpLand2, "sfx");
                jumping = false;
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
    }
    
    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            StandUp();
            state = MovementState.run;
            characterTransform.rotation = Quaternion.identity;
            if (!running && IsGrounded())
            {
                StartCoroutine(Run());
                running = true;
            }
        }
        else if (dirX < 0f)
        {
            StandUp();
            state = MovementState.run;
            characterTransform.rotation = Quaternion.Euler(0f, 180f, 0f);
            if (!running && IsGrounded())
            {
                StartCoroutine(Run());
                running = true;
            }
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

    IEnumerator StartJump()
    {
        running = false;

        yield return new WaitForSeconds(0.5f);
        jumping = true;
    }

    IEnumerator Run()
    {
        audioManager.PlayClip(run1, "sfx");
        yield return new WaitForSeconds(run1.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run2, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run2.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run3, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run3.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run4, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run4.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run5, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run5.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run6, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run6.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run7, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run7.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run8, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run8.length);

        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run9, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }

        yield return new WaitForSeconds(run9.length);
        
        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            audioManager.PlayClip(run10, "sfx");
        }
        else
        {
            running = false;
            yield break;
        }
    }

    void HandleStartGame()
    {
        moveBool = true;
    }

    void HandlePlayerDeath()
    {
        moveBool = false;
    }

    void HandleFinishGame()
    {
        moveBool = false;
        Time.timeScale = 0f;
    }
}
