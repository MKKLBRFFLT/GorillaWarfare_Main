using UnityEngine;

[CreateAssetMenu(fileName = "StoreMenu", menuName = "Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemsSO : ScriptableObject
{
    [Header("Ints")]
    public int price;

    [Header("Strings")]
    public string title;
    public string description;
}
