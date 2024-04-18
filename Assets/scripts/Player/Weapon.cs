using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject shotPrefab;
    public GameObject homingShotPrefab;
    [SerializeField] private AudioClip fireWeaponAudio;
    [SerializeField] AudioClip notEnoughAmmoAudio;
    bool playerIsDead;
    [SerializeField] private float fireRate;
    AudioManager audioManager;
    PlayerMovement playerMovement;
    PlayerCounts playerCounts;
    UIManager uiManager;
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
        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
        playerCounts = playerMovement.GetComponentInChildren<PlayerCounts>();

        if (Input.GetButtonDown("Fire1") && !playerIsDead && playerMovement.moveBool)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time +fireRate;
            if (!uiManager.homingWeaponSelected)
            {
                audioManager.PlayClip(fireWeaponAudio, "sfx");
                Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
            }
            else if (playerCounts.specialAmmo > 0)
            {
                audioManager.PlayClip(fireWeaponAudio, "sfx");
                Instantiate(homingShotPrefab, firePoint.position, firePoint.rotation);
                playerCounts.specialAmmo -= 1;
            }
            else if (uiManager.homingWeaponSelected && playerCounts.specialAmmo == 0)
            {
                audioManager.PlayClip(notEnoughAmmoAudio, "sfx");
            }
        }
    }

    void HandlePlayerDeath()
    {
        playerIsDead = true;
    }
}
