using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Speed,
    FireRate
}

/// <summary>
/// Holds data for each upgrade in the shop (speed, damage, etc). Prices, name, type, stat modifier, etc.
/// </summary>
[System.Serializable]
public class ShopUpgrade
{
    public UpgradeType type;            // Type of upgrade.
    public string displayName;          // Name of the upgrade displayed in the shop.

    public int levels;                  // How many times can we upgrade this stat?
    [HideInInspector]
    public int curLevel;                // Level the player is currently on.

    // Modifier applied to the stat when upgraded.
    // e.g. let's say this upgrade is for Speed.
    //
    //      Default speed = 5
    //      statModifier = 1.2
    //      
    //      level 1 = 6         (5 * 1.5)
    //      level 2 = 7.2       (6 * 1.5)
    //      level 3 = 10.8      (7.2 * 1.5)

    public float statModifier;
    public int[] prices;                // Prices for each level of the upgrade (array needs to be same length as 'levels').
}