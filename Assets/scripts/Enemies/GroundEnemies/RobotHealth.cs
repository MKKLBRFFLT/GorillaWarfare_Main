using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHealth : MonoBehaviour
{
    [Header("Ints")]
    readonly int maxHealth = 3;
    public int health;
    private float yOffset = 0.4f;

    [Header("GameObjects")]
    [SerializeField] GameObject banana;
    [SerializeField] GameObject deathAnimation;
    [SerializeField] GameObject robotHead;

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

            Vector3 headPosition = transform.position + Vector3.up * yOffset; // Offset the position slightly above
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            Instantiate(robotHead, headPosition, transform.rotation); // Instantiate the robotHead at the adjusted position
            Instantiate(banana, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
