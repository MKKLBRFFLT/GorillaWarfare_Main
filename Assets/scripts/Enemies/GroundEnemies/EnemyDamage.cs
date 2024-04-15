using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    #region Events

    public static event Action OnDamage;

    #endregion
    
    #region Variables
    
    [Header("Ints")]
    readonly int damage = 1;

    [Header("Bools")]
    bool cooldown;

    #endregion

    #region StartUpdate

    // Update is called once per frame
    void Update()
    {
        cooldown = GameObject.FindWithTag("Player").transform.Find("Target").GetComponent<PlayerHealth>().cooldown;
    }

    #endregion

    #region Methods

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && coll.gameObject.transform.Find("Target").TryGetComponent<PlayerHealth>(out PlayerHealth playerComp) && !cooldown)
        {
            playerComp.TakeDamage(damage);
            cooldown = true;
            OnDamage?.Invoke();
        }
    }

    #endregion
}
