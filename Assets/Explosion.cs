using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update

    //[SerializeField] private bool isDone;
    [SerializeField] private AudioSource audioPlayer;


    // Update is called once per frame
    void Update()
    {
        if (!audioPlayer.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
