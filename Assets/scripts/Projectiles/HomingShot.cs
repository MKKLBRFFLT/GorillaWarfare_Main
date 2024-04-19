using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class HomingShot : MonoBehaviour
{
    float newDistance;
    float closestDistance = 0f;
    [SerializeField] Transform point;

    readonly float speed = 15f;
    [SerializeField] bool isHoming;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject impactEffect;
    List<GameObject> enemies = new();
    

    void Start()
    {
        rb.velocity = transform.right * speed;
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        foreach (GameObject e in enemies)
        {
            newDistance = Vector2.Distance(e.transform.position, transform.position);

            if (newDistance < closestDistance || closestDistance == 0f)
            {
                closestDistance = newDistance;
                point = e.transform;
            }
        }

        StartCoroutine(HomeToTarget());
    }

    void FixedUpdate()
    {
        if (isHoming)
        {
            rb.isKinematic = true;

            StartCoroutine(MoveTowardsTarget());
        }
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
        if (hitInfo.TryGetComponent<Finish>(out Finish fComp))
        {
            fComp.TakeDamage();
        }

        Destroy(gameObject);
    }

    IEnumerator HomeToTarget()
    {
        yield return new WaitForSeconds(0.1f);

        isHoming = true;
    }

    IEnumerator MoveTowardsTarget()
    {
        float timeInAir = 0f;
        while (true)
        {
            timeInAir += Time.deltaTime;
            float singleStep = timeInAir / 8f;
            transform.position = Vector3.Lerp(transform.position, point.position, singleStep);

            Vector3 targetDirection = point.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0f);

            transform.rotation = Quaternion.LookRotation(newDirection);

            yield return null;
        }
    }
}
