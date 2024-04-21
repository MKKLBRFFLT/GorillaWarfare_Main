using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Events

    public static event Action OnPlayerFastCamActive;
    public static event Action OnBeginGame;

    #endregion
    
    #region Variables

    [Header("Cameras")]
    Camera mainCam;

    [Header("Floats")]
    readonly float moveSpeed = 4f;

    [Header("Bools")]
    [SerializeField] bool mainMenuCamBool;
    [SerializeField] bool playerCamBool;
    [SerializeField] bool playerFastCamBool;

    [Header("GameObjects")]
    GameObject player;
    GameObject mainMenuCamPos;

    #endregion
    
    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        
        player = GameObject.FindGameObjectWithTag("Player");
        mainMenuCamPos = GameObject.FindGameObjectWithTag("MainMenuCamPos");

        mainMenuCamBool = true;
        playerCamBool = false;
        playerFastCamBool = false;
    }

    void FixedUpdate()
    {
        if (mainMenuCamBool)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, mainMenuCamPos.transform.position, moveSpeed * Time.deltaTime);
        }
        if (playerCamBool)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(player.transform.position.x, 0f, -10f), moveSpeed * Time.deltaTime);
        }
        if (playerFastCamBool)
        {
            float playerPos = player.transform.position.x;
            playerPos = Mathf.Clamp(playerPos, -5f, 152);
            
            if (player.transform.position.y >= 18f)
            {
                mainCam.transform.position = new Vector3(playerPos, 22f, -10f);
            }
            else if (player.transform.position.y >= 15f)
            {
                mainCam.transform.position = new Vector3(playerPos, 16f, -10f);
            }
            else if (player.transform.position.y >= 8f)
            {
                mainCam.transform.position = new Vector3(playerPos, 12f, -10f);
            }
            else if (player.transform.position.y >= 3.5f)
            {
                mainCam.transform.position = new Vector3(playerPos, 6f, -10f);
            }
            else
            {
                mainCam.transform.position = new Vector3(playerPos, 0f, -10f);
            }
            
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if ((mainCam.transform.position == Vector3.Lerp(mainCam.transform.position, new Vector3(player.transform.position.x, 0f, -10f), moveSpeed * Time.deltaTime)) && playerCamBool)
        {
            ActivatePlayerFastCam();
        }
    }

    #endregion

    #region Methods

    public void ActivatePlayerCam()
    {
        playerCamBool = true;
        mainMenuCamBool = false;
        OnBeginGame?.Invoke();
    }

    void ActivatePlayerFastCam()
    {
        playerCamBool = false;
        playerFastCamBool = true;
        OnPlayerFastCamActive?.Invoke();
    }

    #endregion
}
