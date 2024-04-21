using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycard : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private float moveSpeed = 150f;

    public static event Action OnPickup;

    RedGateControl doorController;

    private bool isMoving = false;

    void Start()
    {
        doorController = GameObject.FindWithTag("BossRoom").transform.Find("wall (1)/securitygate_vertical (1)").GetComponent<RedGateControl>();

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            
            transform.Translate(direction * moveSpeed * Time.deltaTime*2);

            
            if (Vector3.Distance(transform.position, playerTransform.position) < 1f)
            {
                isMoving = false;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.Find("Target") && coll.transform.Find("Target").TryGetComponent<PlayerCounts>(out PlayerCounts pComp))
        {
            pComp.keycardAmount += 1;
            OnPickup?.Invoke();

            doorController.OpenDoor(); // Trigger the door to start moving down
            
            Destroy(gameObject);
        }
    }
}
