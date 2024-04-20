using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowTrigger : MonoBehaviour
{
    [SerializeField] private YellowGateControl doorController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorController.OpenDoor(); // Trigger the door to start moving down
        }
    }
}
