using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCounts : MonoBehaviour
{
    #region Variables

    public AmmoState aState;
    
    [Header("Ints")]
    public int bananaAmount;
    public int specialAmmo = 10;
    public int keycardAmount;

    [Header("AudioClips")]
    [SerializeField] AudioClip bananaPickupAudio;
    [SerializeField] AudioClip keycardPickupAudio;

    [Header("Components")]
    AudioManager audioManager;

    #endregion

    #region Subscriptions

    void OnEnable()
    {
        banana.OnPickup += HandleBanana;
        keycard.OnPickup += HandleKeycard;
    }

    void OnDisable()
    {
        banana.OnPickup -= HandleBanana;
        keycard.OnPickup -= HandleKeycard;
    }

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindWithTag("Managers").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            switch (aState)
            {
                case AmmoState.normal:
                    aState = AmmoState.special;
                    break;
                
                case AmmoState.special:
                    aState = AmmoState.normal;
                    break;
            }
        }
    }

    #endregion
    
    #region HandleSubscriptions

    void HandleBanana()
    {
        audioManager.PlayClip(bananaPickupAudio, "sfx");
    }

    void HandleKeycard()
    {
        audioManager.PlayClip(keycardPickupAudio, "sfx");
    }

    #endregion

    #region Enums
    
    public enum AmmoState
    {
        normal,
        special
    }

    #endregion
}
