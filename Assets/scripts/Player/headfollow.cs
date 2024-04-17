using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headfollow : MonoBehaviour
{
    private Transform target;
    private SpriteRenderer headRenderer;
    // Start is called before the first frame update
    void Start()
    {
        headRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player").transform.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < target.position.x)
        {
            headRenderer.flipX = true;
        }
        else
        {
            headRenderer.flipX = false;
        }
    }
}
