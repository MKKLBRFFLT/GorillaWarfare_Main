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

    [Header("Bools")]
    public bool cooldown;
    public bool isDead;
    [SerializeField] bool invinsible = false;
    bool animFound;

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
        }
        // print("Player has " + health + " health");
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(damageCooldown);
        cooldown = false;
    }

    IEnumerator PlayerDying()
    {
        yield return new WaitForSeconds(clipLength); // Ændre "2f" til en reference af playerDeath animationens længde.
        
        OnPlayerDeath?.Invoke();
        Time.timeScale = 0f;
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
