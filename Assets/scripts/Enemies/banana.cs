using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana : MonoBehaviour
{
    public static event Action OnPickup;

    //private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.Find("Target") && coll.transform.Find("Target").TryGetComponent<PlayerCounts>(out PlayerCounts pComp))
        {
            pComp.bananaAmount += 1;
            //spriteRenderer.enabled = false;
            OnPickup?.Invoke();
            Destroy(gameObject);
        }
    }
}
