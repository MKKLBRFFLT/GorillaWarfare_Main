using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField]
    public bool isFlipped;
    private Transform characterTransform;

    // Start is called before the first frame update
    void Start()
    {
        characterTransform = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

        if (isFlipped)
        {
            characterTransform.rotation = Quaternion.Euler(0f, 180f, 0f);

        }
        else
        {
            characterTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
