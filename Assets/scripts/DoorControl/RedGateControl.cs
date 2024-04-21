using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGateControl : MonoBehaviour
{
    [SerializeField] private Transform endPoint; 
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private AudioSource audioSource;


    private bool isOpening = false;

    void Update()
    {
        if (isOpening && transform.position.y < endPoint.position.y)
        {
            
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
