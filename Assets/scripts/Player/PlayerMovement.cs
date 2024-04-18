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

    int material;

    private float dirX = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] AudioClip jumpGroundStart2;
    [SerializeField] AudioClip jumpGroundLand2;
    [SerializeField] AudioClip jumpObstructionStart2;
    [SerializeField] AudioClip jumpObstructionLand2;
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
    [SerializeField] AudioClip runOb1;
    [SerializeField] AudioClip runOb2;
    [SerializeField] AudioClip runOb3;
    [SerializeField] AudioClip runOb4;
    [SerializeField] AudioClip runOb5;
    [SerializeField] AudioClip runOb6;
    [SerializeField] AudioClip runOb7;
    [SerializeField] AudioClip runOb8;
    [SerializeField] AudioClip runOb9;
    [SerializeField] AudioClip runOb10;

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
                    
                    if (material == 1)
                    {
                        audioManager.PlayClip(jumpGroundStart2, "sfx");
                    }
                    else if (material == 2)
                    {
                        audioManager.PlayClip(jumpObstructionStart2, "sfx");
                    }
                    StartCoroutine(StartJump());
                }
            }

            if (jumping && IsGrounded())
            {
                if (material == 1)
                {
                    audioManager.PlayClip(jumpGroundLand2, "sfx");
                }
                else if (material == 2)
                {
                    audioManager.PlayClip(jumpObstructionLand2, "sfx");
                }
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (IsGrounded())
        {
            if (coll.transform.CompareTag("Ground"))
            {
                material = 1;
            }
            else if (coll.transform.CompareTag("Obstruction") || coll.transform.CompareTag("Terrain"))
            {
                material = 2;
            }
        }
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

    void RunCheck(AudioClip ground, AudioClip metal)
    {
        if ((dirX < 0f || dirX > 0f) && IsGrounded())
        {
            if (material == 1)
            {
                audioManager.PlayClip(ground, "sfx");
            }
            else if (material == 2)
            {
                audioManager.PlayClip(metal, "sfx");
            }
        }
    }

    float RunCheckTimer(AudioClip ground, AudioClip metal)
    {
        if (material == 1)
        {
            return ground.length;
        }
        else if (material == 2)
        {
            return metal.length;
        }
        else
        {
            return 0f;
        }
    }

    IEnumerator Run()
    {
        RunCheck(run1, runOb1);
        yield return new WaitForSeconds(RunCheckTimer(run1, runOb1));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run2, runOb2);
        yield return new WaitForSeconds(RunCheckTimer(run2, runOb2));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run3, runOb3);
        yield return new WaitForSeconds(RunCheckTimer(run3, runOb3));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run4, runOb4);
        yield return new WaitForSeconds(RunCheckTimer(run4, runOb4));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run5, runOb5);
        yield return new WaitForSeconds(RunCheckTimer(run5, runOb5));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run6, runOb6);
        yield return new WaitForSeconds(RunCheckTimer(run6, runOb6));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run7, runOb7);
        yield return new WaitForSeconds(RunCheckTimer(run7, runOb7));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run8, runOb8);
        yield return new WaitForSeconds(RunCheckTimer(run8, runOb8));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run9, runOb9);
        yield return new WaitForSeconds(RunCheckTimer(run9, runOb9));
        if (dirX == 0f || !IsGrounded())
        {
            running = false;
            yield break;
        }

        RunCheck(run10, runOb10);
        yield return new WaitForSeconds(RunCheckTimer(run10, runOb10));

        running = false;
        yield break;
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
