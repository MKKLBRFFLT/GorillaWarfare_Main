using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private float parallaxEffectMultiplier;
    [SerializeField] bool beginGame;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void OnEnable()
    {
        if (!beginGame)
        {
            CameraManager.OnPlayerFastCamActive += HandleStartGame;
        }
    }

    void OnDisable()
    {
        if (!beginGame)
        {
            CameraManager.OnPlayerFastCamActive -= HandleStartGame;
        }
    }

    private void Start()
    {
        if (beginGame)
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
        }
    }

    private void LateUpdate()
    {
        if (beginGame)
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += deltaMovement * parallaxEffectMultiplier;
            lastCameraPosition = cameraTransform.position;
        }
    }

    void HandleStartGame()
    {
        beginGame = true;
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }
}
