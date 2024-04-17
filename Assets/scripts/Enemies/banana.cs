using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioPlayer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.Find("Target") && coll.transform.Find("Target").TryGetComponent<PlayerCounts>(out PlayerCounts pComp))
        {
            pComp.bananaAmount += 1;
            spriteRenderer.enabled = false;
            audioPlayer.Play();
        }


    }
    private void PlayAudioAndDestroy()
    {
        audioPlayer.Play();
        StartCoroutine(DestroyAfterDelay(audioPlayer.clip.length));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
