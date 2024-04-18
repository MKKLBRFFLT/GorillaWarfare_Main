using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public static event Action OnLevelComplete;

    public void TakeDamage()
    {
        OnLevelComplete?.Invoke();
        Destroy(gameObject);
    }
}
