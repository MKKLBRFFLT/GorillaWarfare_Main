using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHealth : MonoBehaviour
{
    [Header("Ints")]
    readonly int maxHealth = 1;
    public int health;

    [Header("GameObjects")]
    [SerializeField] GameObject turret;
    [SerializeField] GameObject deathAnim;

    
    public event Action<int> OnBatteryDestroyed;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            
            Instantiate(deathAnim, transform.position, Quaternion.identity); 
            OnBatteryDestroyed?.Invoke(1);
            Destroy(gameObject);
            
        }


    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
