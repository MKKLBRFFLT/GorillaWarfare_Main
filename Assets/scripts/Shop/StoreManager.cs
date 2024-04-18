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

    [Header("Bools")]
    public bool storeOpen;
    bool pannelsLoaded;
    bool playerMoveStoped;

    [Header("TMP_Pros")]
    public TMP_Text currencyText;
    
    [Header("Arrays")]
    public ShopItemsSO[] shopItemsSO;
    public ShopTemplate[] shopPannels;
    public GameObject[] shopPannelsSO;
    public Button[] purchaseShopButtons;

    [Header("GameObjects")]
    [SerializeField] GameObject contents;

    // Start is called before the first frame update
    void Start()
    { 
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPannelsSO[i].SetActive(true);
        }

        CheckShopPurchaseable();
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
            if (playerMoveStoped)
            {
                Time.timeScale = 1f;
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
}
