using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headfollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private SpriteRenderer headRenderer;
    // Start is called before the first frame update
    void Start()
    {
        headRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < target.transform.position.x)
        {
            headRenderer.flipX = true;
        }
        else
        {
            headRenderer.flipX = false;
        }
    }
}
