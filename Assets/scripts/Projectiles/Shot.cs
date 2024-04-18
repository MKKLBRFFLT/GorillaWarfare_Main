using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject impactEffect;
    

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);

        if (hitInfo.TryGetComponent<TurretHealth>(out TurretHealth tComp))
        {
            tComp.TakeDamage(1);
        }
        if (hitInfo.TryGetComponent<RobotHealth>(out RobotHealth rComp))
        {
            rComp.TakeDamage(1);
        }
        if (hitInfo.TryGetComponent<SlimeHealth>(out SlimeHealth sComp))
        {
            sComp.TakeDamage(1);
        }
        

        Destroy(gameObject);

    }

    
}
