using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGateControl : MonoBehaviour
{
    [SerializeField] private Transform endPoint; // The position where the door should stop moving
    [SerializeField] private float moveSpeed = 20f;

    private bool isOpening = false;

    void Update()
    {
        if (isOpening && transform.position.y > endPoint.position.y)
        {
            // Move the door down towards the endpoint
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
    }
}
