using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    #region Events

    public static event Action OnPlayerDeath;

    #endregion
    
    #region  Variables

    [Header("Ints")]
    readonly int maxHealth = 3;
    public int health;
    
    [Header("Floats")]
    readonly float damageCooldown = 1f;
    float clipLength;
    float blinkTime = 0.2f;

    [Header("Bools")]
    public bool cooldown;
    public bool isDead;
    [SerializeField] bool invinsible = false;
    bool animFound;

    [Header("SpriteRenderers")]
    [SerializeField] SpriteRenderer head;
    [SerializeField] SpriteRenderer frontArm;
    [SerializeField] SpriteRenderer torso;
    [SerializeField] SpriteRenderer leftLeg;
    [SerializeField] SpriteRenderer rightLeg;
    [SerializeField] SpriteRenderer bazookaArm;

    [Header("Components")]
    PlayerMovement playerMovement;

    #endregion

    #region Event Subscribtions
    
    void OnEnable()
    {
        BulletScript.OnDamage += HandleDamage;
        EnemyDamage.OnDamage += HandleDamage;
        
    }

    void OnDisable()
    {
        BulletScript.OnDamage -= HandleDamage;
        EnemyDamage.OnDamage -= HandleDamage;
    }

    #endregion

    #region StartUpdate
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        
        health = maxHealth;
        // print("Player has " + health + " health");

        cooldown = false;
        isDead = false;

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();

        if (!animFound)
        {
            AnimationClip[] clips = playerMovement.anim.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips)
            {
                if (clip.name == "death")
                {
                    clipLength = clip.length;
                }
            }

            animFound = true;
        }
    }

    #endregion

    #region Methods
    
    void CheckHealth()
    {
        if (health <= 0 && !isDead)
        {
            StartCoroutine(PlayerDying());
            isDead = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!invinsible)
        {
            health -= damage;

            if (health > 0)
            {
                StartCoroutine(DamageTaken());
            }
        }
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(damageCooldown);
        cooldown = false;
    }

    IEnumerator PlayerDying()
    {
        yield return new WaitForSeconds(clipLength);
        
        OnPlayerDeath?.Invoke();
        Time.timeScale = 0f;
    }

    IEnumerator DamageTaken()
    {
        head.color = Color.red;
        torso.color = Color.red;
        frontArm.color = Color.red;
        leftLeg.color = Color.red;
        rightLeg.color = Color.red;
        bazookaArm.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        head.color = Color.white;
        torso.color = Color.white;
        frontArm.color = Color.white;
        leftLeg.color = Color.white;
        rightLeg.color = Color.white;
        bazookaArm.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(DamageCooldown());
    }

    IEnumerator DamageCooldown()
    {
        StartCoroutine(Blink());

        yield return new WaitForSeconds(blinkTime * 2);

        StartCoroutine(Blink());
        
        yield return new WaitForSeconds(blinkTime * 2);

        StartCoroutine(Blink());

        yield return new WaitForSeconds(blinkTime * 2);

        StartCoroutine(Blink());

        yield return new WaitForSeconds(blinkTime * 2);

        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        head.enabled = false;
        torso.enabled = false;
        frontArm.enabled = false;
        leftLeg.enabled = false;
        rightLeg.enabled = false;
        bazookaArm.enabled = false;

        yield return new WaitForSeconds(blinkTime);

        head.enabled = true;
        torso.enabled = true;
        frontArm.enabled = true;
        leftLeg.enabled = true;
        rightLeg.enabled = true;
        bazookaArm.enabled = true;
    }

    #endregion

    #region Subribtion Handlers

    void HandleDamage()
    {
        StopCoroutine(nameof(CooldownRoutine));
        cooldown = true;
        StartCoroutine(CooldownRoutine());
    }

    #endregion
}
