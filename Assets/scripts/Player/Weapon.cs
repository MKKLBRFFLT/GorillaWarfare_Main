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
    PlayerMovement playerMovement;
    private float nextFire;
    
    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }

    void Start()
    {
        playerIsDead = false;
        audioManager = GameObject.FindWithTag("Managers").GetComponent<AudioManager>();
    }

    void Update()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();

        if (Input.GetButtonDown("Fire2") && !playerIsDead && playerMovement.moveBool)
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

    void HandlePlayerDeath()
    {
        playerIsDead = true;
    }
}
