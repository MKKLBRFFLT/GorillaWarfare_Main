using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHead : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        color.a -= fadeSpeed * Time.deltaTime;

        color.a = Mathf.Clamp01(color.a);
        spriteRenderer.color = color;

        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
