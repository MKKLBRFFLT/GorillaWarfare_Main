using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniTurretHealth : MonoBehaviour
{
    [Header("Ints")]
    readonly int maxHealth = 3;
    public int health;

    [Header("GameObjects")]
    [SerializeField] GameObject turret;
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
            Instantiate(deathAnim, transform.position, Quaternion.identity);
            Instantiate(banana, transform.position, Quaternion.identity);
            Destroy(turret);
        }


    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<BatteryHealth>(out BatteryHealth battery))
            {
                battery.OnBatteryDestroyed += OnBatteryDestroyed;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<BatteryHealth>(out BatteryHealth battery))
            {
                battery.OnBatteryDestroyed -= OnBatteryDestroyed;
            }
        }
    }

    void OnBatteryDestroyed(int damage)
    {
        TakeDamage(damage);
    }
}
