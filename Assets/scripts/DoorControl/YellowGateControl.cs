using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGateControl : MonoBehaviour
{
    [SerializeField] private Transform endPoint; 
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private AudioSource audioSource;


    private bool isClosing = false;

    void Update()
    {
        if (isClosing && transform.position.y > endPoint.position.y)
        {
            
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    public void CloseDoor()
    {
        isClosing = true;

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
