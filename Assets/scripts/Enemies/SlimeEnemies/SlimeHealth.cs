using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealth : MonoBehaviour
{
    [Header("Ints")]
    readonly int maxHealth = 1;
    public int health;

    [Header("GameObjects")]
    [SerializeField] GameObject banana;
    [SerializeField] GameObject deathAnim;

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
            Instantiate(deathAnim, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
