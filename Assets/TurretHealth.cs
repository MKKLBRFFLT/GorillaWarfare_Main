using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    [Header("Ints")]
    readonly int maxHealth = 1;
    public int health;

    [Header("GameObjects")]
    [SerializeField] GameObject turret;
    [SerializeField] GameObject banana;

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
            Instantiate(banana, transform.position, Quaternion.identity);
            Destroy(turret);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
