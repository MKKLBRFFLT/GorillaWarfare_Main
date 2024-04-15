using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject shotPrefab;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private float fireRate;
    private float nextFire;


    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }

        
    }
    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time +fireRate;
            audioPlayer.Play();
            Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
        }
        
    }
}
