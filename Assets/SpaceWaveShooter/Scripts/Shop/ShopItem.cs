using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds data for each item you can buy in the shop.
/// </summary>
[System.Serializable]
public class ShopItem
{
    public string itemName;                 // Name of the item.
    public int price;                       // Cost to purchase.
    public GameObject prefabToSpawn;        // Prefab to spawn when the player purchases the item.

    [HideInInspector]
    public bool purchased;                  // Has the player purchased the item?
}