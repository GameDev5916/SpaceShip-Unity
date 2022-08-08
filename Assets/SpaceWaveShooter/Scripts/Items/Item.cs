using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for items purchasable from the shop.
/// </summary>
public class Item : MonoBehaviour
{
    private int itemIndex;      // Index of the item in the shop's 'items' array.

    void Start ()
    {
        Initialize();
    }

    // Called when the item is initially created.
    public virtual void Initialize ()
    {

    }

    // Called when the item is created.
    // Set some properties from the shop.
    public void SetProperties (int index)
    {
        itemIndex = index;
    }

    // Called when the item is destroyed.
    public void DestroyItem ()
    {
        // We want to tell the shop that the player can buy this item again.
        Shop.inst.OnItemDestroyed(itemIndex);

        Destroy(gameObject);
    }
}