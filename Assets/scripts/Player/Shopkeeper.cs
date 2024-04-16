using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    #region Variables

    [Header("Strings")]
    readonly string promtString = "Open shop (Press E)";

    [Header("Components")]
    StoreManager store;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        store = GameObject.FindWithTag("MainUI").transform.Find("StoreMenu").GetComponent<StoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Methods

    public string promt => promtString;

    public bool Interact(Interactor interactor)
    {
        store.OpenStore();
        return true;
    }

    #endregion
}
