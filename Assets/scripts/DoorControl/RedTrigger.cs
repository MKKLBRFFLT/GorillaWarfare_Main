using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTrigger : MonoBehaviour
{
    [SerializeField] private RedGateControl doorController;

    void Start()
    {
        doorController = GameObject.FindWithTag("BossRoom").transform.Find("wall (1)/securitygate_vertical (1)").GetComponent<RedGateControl>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorController.OpenDoor(); // Trigger the door to start moving down
        }
    }
}
