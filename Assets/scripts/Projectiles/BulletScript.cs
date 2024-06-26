using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    #region Events

    public static event Action OnDamage;

    #endregion

    #region Variables
    
    [Header("Ints")]
    readonly int damage = 1;

    [Header("Floats")]
    readonly float despawnTime = 3f;

    [Header("Bools")]
    bool cooldown;

    [Header("Components")]
    Rigidbody2D rb;

    #endregion

    #region StartUpdate
    
    void Awake()
    {
        StartCoroutine(DespawnTimer());
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = GameObject.FindWithTag("Player").transform.Find("Target").GetComponent<PlayerHealth>().cooldown;
    }

    #endregion

    #region Methods
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.transform.Find("Target") && coll.gameObject.transform.Find("Target").TryGetComponent<PlayerHealth>(out PlayerHealth playerComp) && !cooldown)
        {
            playerComp.TakeDamage(damage);
            OnDamage?.Invoke();
        }
        Destroy(gameObject);

        // print("bullet collided);
    }

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
        
        // print("bullet despawned");
    }

    #endregion
}
