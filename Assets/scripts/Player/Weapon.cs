using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject shotPrefab;
    [SerializeField] private AudioClip fireWeaponAudio;
    bool playerIsDead;
    [SerializeField] private float fireRate;
    AudioManager audioManager;
    private float nextFire;

    #region Event Subscribtions
    
    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }

    #endregion

    void Start()
    {
        playerIsDead = false;
        audioManager = GameObject.FindWithTag("Managers").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && !playerIsDead)
        {
            Shoot();
        }

        
    }
    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time +fireRate;
            audioManager.PlayClip(fireWeaponAudio, "sfx");
            Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
        }
        
    }

    #region Subribtion Handlers

    void HandlePlayerDeath()
    {
        playerIsDead = true;
    }

    #endregion
}
