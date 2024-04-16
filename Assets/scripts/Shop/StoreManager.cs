using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class StoreManager : MonoBehaviour
{
    [Header("Floats")]
    public float currency;
    public float SFXVolume;
    public float masterVolume;
    public float realVolume;

    [Header("Bools")]
    public bool storeOpen;
    bool pannelsLoaded;
    bool playerMoveStoped;

    [Header("Ints")]
    [SerializeField] private int InitialAudioSourcePoolSize = 10;

    [Header("TMP_Pros")]
    public TMP_Text currencyText;

    [Header("Lists")]
    private List<AudioSource> audioSourcePool = new();
    
    [Header("Arrays")]
    public ShopItemsSO[] shopItemsSO;
    public ShopTemplate[] shopPannels;
    public GameObject[] shopPannelsSO;
    public Button[] purchaseShopButtons;

    [Header("GameObjects")]
    [SerializeField] GameObject contents;

    [Header("Components")]
    [SerializeField] AudioClip audioBuy;
    [SerializeField] AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    { 
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPannelsSO[i].SetActive(true);
        }

        CheckShopPurchaseable();

        for (int i = 0; i < InitialAudioSourcePoolSize; i++)
        {
            _ = AddNewSourceToPool();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currency = GameObject.FindWithTag("Managers").GetComponent<UIManager>().bananaAmount;
        
        if (Input.GetKeyDown(KeyCode.Escape) && storeOpen)
        {
            CloseStore();
        }
        
        if (!pannelsLoaded)
        {
            LoadShopPannels();
        }

        CheckShopPurchaseable();

        if (storeOpen)
        {
            contents.SetActive(true);
            Time.timeScale = 0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().moveBool = false;
            playerMoveStoped = true;
        }
        else
        {
            contents.SetActive(false);
            Time.timeScale = 1f;
            if (playerMoveStoped)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().moveBool = true;
                playerMoveStoped = false;
            }
        }
    }

    private void LoadShopPannels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPannels[i].titleText.text = shopItemsSO[i].title;
            shopPannels[i].descriptionText.text = shopItemsSO[i].description;
            shopPannels[i].priceText.text = "Price: " + shopItemsSO[i].price.ToString();
        }

        pannelsLoaded = true;
    }

    private void CheckShopPurchaseable()
    {
        currencyText.text = $"{currency}";

        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (currency >= shopItemsSO[i].price)
            {
                if (shopItemsSO[i].title == "Health")
                {
                    if (GameObject.FindWithTag("Target").GetComponent<PlayerHealth>().health < 3)
                    {
                        purchaseShopButtons[i].interactable = true;
                    }
                    else
                    {
                        purchaseShopButtons[i].interactable = false;
                    }
                }
                else
                {
                    purchaseShopButtons[i].interactable = true;
                }
            }
            else
            {
                purchaseShopButtons[i].interactable = false;
            }
        }
    }

    public void PurchaseShopItem(int btnNoShop)
    {
        if (currency >= shopItemsSO[btnNoShop].price)
        {
            GameObject.FindWithTag("Target").GetComponent<PlayerCounts>().bananaAmount -= shopItemsSO[btnNoShop].price;
            currency = GameObject.FindWithTag("Managers").GetComponent<UIManager>().bananaAmount;
            // PlayClip(audioBuy);

            //Unlock purchased item.
            if (shopItemsSO[btnNoShop].title == "Health")
            {
                GameObject.FindWithTag("Target").GetComponent<PlayerHealth>().health += 1;
            }
            else if (shopItemsSO[btnNoShop].title == "Special Ammo")
            {
                GameObject.FindWithTag("Target").GetComponent<PlayerCounts>().specialAmmo += 1;
            }
        }
    }

    public void OpenStore()
    {
        storeOpen = true;
    }

    public void CloseStore()
    {
        storeOpen = false;
    }

    private AudioSource AddNewSourceToPool()
    {
        audioMixer.GetFloat("SFXVolume", out float dBSFX);
        SFXVolume = Mathf.Pow(10.0f, dBSFX / 20.0f);

        audioMixer.GetFloat("masterVolume", out float dBMaster);
        masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
        
        realVolume = (SFXVolume + masterVolume) / 2 * 0.05f;
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = 0.05f;
        audioSourcePool.Add(newSource);
        return newSource;
    }

    private AudioSource GetAvailablePoolSource()
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
 
        //No unused sources. Create and fetch a new source
        return AddNewSourceToPool();
    }

    public void PlayClip(AudioClip clip)
    {
        AudioSource source = GetAvailablePoolSource();
        source.clip = clip;
        source.Play();
    }
}
