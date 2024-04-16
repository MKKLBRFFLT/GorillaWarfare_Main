using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField] AmmoState aState;
    
    [Header("Ints")]
    public int bananaAmount;
    int health;
    int specialAmmo;

    [Header("Floats")]
    [SerializeField] float ammoObjMoveSpeed = 0.1f;
    float ammoObjDistance;

    [Header("Bools")]
    bool elementsFound;
    
    [Header("TMPs")]
    TMP_Text bananaText;
    TMP_Text specialAmmoText;
    TMP_Text prompt;

    [Header("GameObjects")]
    GameObject canvas;
    GameObject health1Obj;
    GameObject health2Obj;
    GameObject health3Obj;
    GameObject ammoObj;
    GameObject specialAmmoObj;
    GameObject deathMsgObj;
    GameObject promtText;
    
    [Header("Transforms")]
    Transform ammoFront;
    Transform ammoBack;
    Transform player;

    [Header("Images")]
    Image healthImg1;
    Image healthImg2;
    Image healthImg3;

    [Header("Colors")]
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    #endregion

    #region Subscriptions

    void OnEnable()
    {
        CameraManager.OnPlayerFastCamActive += HandleStartGame;
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        CameraManager.OnPlayerFastCamActive -= HandleStartGame;
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }

    #endregion
    
    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        CameraManager camManager = GameObject.FindWithTag("Managers").GetComponent<CameraManager>();
        canvas = GameObject.FindGameObjectWithTag("MainUI");
        
        if (camManager.isActiveAndEnabled)
        {
            canvas.SetActive(false);
        }
        else
        {
            FindElements();
        }
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bananaAmount = player.Find("Target").GetComponent<PlayerCounts>().bananaAmount;
        health = player.Find("Target").GetComponent<PlayerHealth>().health;
        specialAmmo = player.Find("Target").GetComponent<PlayerCounts>().specialAmmo;
        aState = (AmmoState)player.Find("Target").GetComponent<PlayerCounts>().aState;

        if (elementsFound)
        {
            UpdateBananaText();
            UpdateHealthBar();
            UpdateSpecialAmmo();
            UpdateAmmoType();
            UpdatePromt();
        }
    }

    #endregion

    #region Methods

    void FindElements()
    {
        bananaText = canvas.transform.Find("UIElements/Bananas/BananasText (TMP)").GetComponent<TextMeshProUGUI>();
        specialAmmoText = canvas.transform.Find("UIElements/Ammo/SpecialAmmo/SpecialAmmo (TMP)").GetComponent<TextMeshProUGUI>();

        health1Obj = canvas.transform.Find("UIElements/Health/Health 1").gameObject;
        health2Obj = canvas.transform.Find("UIElements/Health/Health 2").gameObject;
        health3Obj = canvas.transform.Find("UIElements/Health/Health 3").gameObject;

        ammoObj = canvas.transform.Find("UIElements/Ammo/Ammo").gameObject;
        specialAmmoObj = canvas.transform.Find("UIElements/Ammo/SpecialAmmo").gameObject;
        ammoFront = canvas.transform.Find("UIElements/Ammo/AmmoFrontPos").transform;
        ammoBack = canvas.transform.Find("UIElements/Ammo/AmmoBackPos").transform;

        healthImg1 = health1Obj.GetComponent<Image>();
        healthImg2 = health2Obj.GetComponent<Image>();
        healthImg3 = health3Obj.GetComponent<Image>();

        ammoObjDistance = Vector3.Distance(ammoBack.position, ammoFront.position);

        deathMsgObj = canvas.transform.Find("UIMenus/DeathMsgPanel").gameObject;
        promtText = canvas.transform.Find("UIElements/PromtText (TMP)").gameObject;
        
        deathMsgObj.SetActive(false);

        elementsFound = true;
    }

    #endregion

    #region ButtonMethods

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
    
    #region UpdateMethods

    void UpdateBananaText()
    {
        bananaText.text = $"{bananaAmount}";
    }

    void UpdateHealthBar()
    {
        switch (health)
        {
            case 3:
                healthImg1.fillAmount = 1;
                healthImg2.fillAmount = 1;
                healthImg3.fillAmount = 1;
                break;
            
            case 2:
                healthImg1.fillAmount = 1;
                healthImg2.fillAmount = 1;
                healthImg3.fillAmount = 0;
                break;
            
            case 1:
                healthImg1.fillAmount = 1;
                healthImg2.fillAmount = 0;
                healthImg3.fillAmount = 0;
                break;

            case 0:
                healthImg1.fillAmount = 0;
                healthImg2.fillAmount = 0;
                healthImg3.fillAmount = 0;
                break;
        }
    }

    void UpdateSpecialAmmo()
    {
        specialAmmoText.text = $"x{specialAmmo}";
    }

    void UpdateAmmoType()
    {
        switch (aState)
        {
            case AmmoState.normal:
                StopCoroutine(nameof(AmmoToBack));
                StartCoroutine(AmmoToFront());
                break;

            case AmmoState.special:
                StopCoroutine(nameof(AmmoToFront));
                StartCoroutine(AmmoToBack());
                break;
        }
    }

    void UpdatePromt()
    {
        if (player.GetComponentInChildren<Interactor>().promtFound)
        {
            promtText.SetActive(true);
            promtText.GetComponent<TMP_Text>().text = $"{player.GetComponentInChildren<Interactor>().colliders[0].GetComponent<IInteractable>().promt}";
        }
        else
        {
            promtText.SetActive(false);
        }
    }

    #endregion

    #region Coroutines

    IEnumerator AmmoToFront()
    {
        ammoObj.transform.position = Vector3.Lerp(ammoObj.transform.position, ammoFront.position, ammoObjMoveSpeed);
        
        specialAmmoObj.transform.position = Vector3.Lerp(specialAmmoObj.transform.position, ammoBack.position, ammoObjMoveSpeed);
        
        yield return new WaitForSeconds(ammoObjDistance * ammoObjMoveSpeed);

        ammoObj.transform.SetAsLastSibling();
        ammoObj.GetComponent<Image>().color = activeColor;

        specialAmmoObj.transform.SetAsFirstSibling();
        specialAmmoObj.GetComponent<Image>().color = inactiveColor;
    }

    IEnumerator AmmoToBack()
    {
        ammoObj.transform.position = Vector3.Lerp(ammoObj.transform.position, ammoBack.position, ammoObjMoveSpeed);
        
        specialAmmoObj.transform.position = Vector3.Lerp(specialAmmoObj.transform.position, ammoFront.position, ammoObjMoveSpeed);
        
        yield return new WaitForSeconds(ammoObjDistance * ammoObjMoveSpeed);

        ammoObj.transform.SetAsFirstSibling();
        ammoObj.GetComponent<Image>().color = inactiveColor;

        specialAmmoObj.transform.SetAsLastSibling();
        specialAmmoObj.GetComponent<Image>().color = activeColor;
    }

    #endregion

    #region HandleMethods

    void HandleStartGame()
    {
        canvas.SetActive(true);
        FindElements();
    }

    void HandlePlayerDeath()
    {
        deathMsgObj.SetActive(true);
    }

    #endregion

    #region Enums
    
    enum AmmoState
    {
        normal,
        special
    }

    #endregion
}
