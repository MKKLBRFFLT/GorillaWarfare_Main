using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.Find("Target") && coll.transform.Find("Target").TryGetComponent<PlayerCounts>(out PlayerCounts pComp))
        {
            pComp.bananaAmount += 1;
            Destroy(gameObject);
        }
    }
}
