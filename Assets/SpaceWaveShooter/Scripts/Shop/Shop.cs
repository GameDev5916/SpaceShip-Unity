using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopObject;

    [Header("Shop Contents")]
    public ShopItem[] items;                    // All the available items in the shop.
    public ShopUpgrade[] upgrades;              // All the available upgrades in the shop.

    [Header("Items UI")]
    public Button[] itemButtons;                // All the buttons used for items.

    [Header("Upgrades UI")]
    public Button[] upgradeButtons;             // All the buttons used for upgrades.
    private Text[] upgradePriceText;            // Text for all the upgrade buttons (cached when the game starts).
    private Image[] upgradeProgressBars;        // Progress bar images for all the upgrade buttons (cached when the game starts).

    // Instance
    public static Shop inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;
    }

    #region Subscribing to Events

    void OnEnable ()
    {
        GameManager.inst.onBeginWave.AddListener(OnBeginWave);
    }

    void OnDisable ()
    {
        GameManager.inst.onBeginWave.RemoveListener(OnBeginWave);
    }

    #endregion

    void Start ()
    {
        InitializeUpgradeButtons();
        InitializeItemButtons();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);
        }
    }
    // Initializes the upgrade buttons.
    // Name, button event, price, progress bar, etc.
    void InitializeUpgradeButtons ()
    {
        // Initialize the cached objects arrays.
        upgradePriceText = new Text[upgrades.Length];
        upgradeProgressBars = new Image[upgrades.Length];

        // Loop through all the upgrade buttons.
        for(int x = 0; x < upgradeButtons.Length; ++x)
        {
            // Enable or disable the upgrade button, depending on if we need it.
            if(x < upgrades.Length)
                upgradeButtons[x].gameObject.SetActive(true);
            else
            {
                upgradeButtons[x].gameObject.SetActive(false);
                continue;
            }

            // Cache the upgrade button objects.
            upgradePriceText[x] = upgradeButtons[x].transform.Find("Price").GetComponent<Text>();
            upgradeProgressBars[x] = upgradeButtons[x].transform.Find("Progress").GetComponent<Image>();
            
            // Set the button's visual properties.
            upgradeButtons[x].transform.Find("UpgradeName").GetComponent<Text>().text = upgrades[x].displayName;
            upgradePriceText[x].text = upgrades[x].prices[0].ToString();
            upgradeProgressBars[x].fillAmount = 0.0f;

            // Set the button's OnClick event.
            int upgradeIndex = x;
            upgradeButtons[x].onClick.AddListener(() => { OnClickUpgradeButton(upgradeIndex); });
        }
    }

    // Initializes and creates the item buttons.
    // Name, price, button event, etc.
    void InitializeItemButtons ()
    {
        // Loop through all the item buttons.
        for(int x = 0; x < itemButtons.Length; ++x)
        {
            // Enable or disable the item button, depending on if we need it.
            if(x < items.Length)
                itemButtons[x].gameObject.SetActive(true);
            else
            {
                itemButtons[x].gameObject.SetActive(false);
                continue;
            }

            // Set the button's visual properties.
            itemButtons[x].transform.Find("UpgradeName").GetComponent<Text>().text = items[x].itemName;
            itemButtons[x].transform.Find("Price").GetComponent<Text>().text = items[x].price.ToString();

            // Set the button's OnClick event.
            int itemIndex = x;
            itemButtons[x].onClick.AddListener(() => { OnClickItemButton(itemIndex); });
        }
    }

    // Opens or closes the shop.
    public void ToggleShop ()
    {
        shopObject.SetActive(!shopObject.activeInHierarchy);
    }

    // Updates the UI elements of the requested upgrade button.
    // Called when the player purchases an upgrade.
    void UpdateUpgradeButton (int upgradeIndex)
    {
        // Are there any more levels that we can upgrade?
        if(upgrades[upgradeIndex].curLevel < upgrades[upgradeIndex].levels)
            upgradePriceText[upgradeIndex].text = upgrades[upgradeIndex].prices[upgrades[upgradeIndex].curLevel].ToString();
        // If not, show that the upgrade is maxed out.
        else
        {
            upgradePriceText[upgradeIndex].text = "MAX";
            upgradeProgressBars[upgradeIndex].color = Color.green;
        }

        // Set the fill amount.
        float mod = 1.0f / (float)upgrades[upgradeIndex].levels;
        upgradeProgressBars[upgradeIndex].fillAmount = mod * upgrades[upgradeIndex].curLevel;
    }

    // Updates the UI elements of the requested item button.
    // Called when the player purchases or destroys an item.
    void UpdateItemButton (int itemIndex)
    {
        // Set the button color.
        ColorBlock colors = itemButtons[itemIndex].colors;

        colors.normalColor = items[itemIndex].purchased ? Color.green : Color.white;
        colors.highlightedColor = items[itemIndex].purchased ? Color.green : Color.white;
        colors.pressedColor = items[itemIndex].purchased ? Color.green : Color.white;

        itemButtons[itemIndex].colors = colors;
    }

    // Called when a player clicks an upgrade button.
    // Sends over the corresponding index in the 'upgrades' array for that upgrade.
    public void OnClickUpgradeButton (int upgradeIndex)
    {
        // Get the upgrade.
        ShopUpgrade upgrade = upgrades[upgradeIndex];

        // Is there an available upgrade to purchase?
        if(upgrade.curLevel < upgrade.levels)
        {
            // Does the player have enough money to purchase the upgrade?
            if(GameManager.inst.curPlayerMoney >= upgrade.prices[upgrade.curLevel])
            {
                PurchaseUpgrade(upgradeIndex);
            }
        }
    }

    // Called when the player purchases an upgrade.
    // Sends over the corresponding index in the 'upgrades' array for that upgrade.
    void PurchaseUpgrade (int upgradeIndex)
    {
        // Get the upgrade.
        ShopUpgrade upgrade = upgrades[upgradeIndex];

        // Take away the money from the player.
        GameManager.inst.TakeMoney(upgrade.prices[upgrade.curLevel]);

        // Give the player the corresponding stat increase.
        PlayerController.inst.UpgradeStat(upgrade.type, upgrade.statModifier);

        // Increase current level.
        upgrade.curLevel++;
        UpdateUpgradeButton(upgradeIndex);
    }

    // Called when a player clicks an item button.
    public void OnClickItemButton (int itemIndex)
    {
        // Get the item.
        ShopItem item = items[itemIndex];

        // Can we purchase the item?
        if(!item.purchased && GameManager.inst.curPlayerMoney >= item.price)
        {
            PurchaseItem(itemIndex);
        }
    }

    // Called when the player purchases an item.
    // Sends over the corresponding index in the 'items' array for that item.
    void PurchaseItem (int itemIndex)
    {
        // Get the item.
        ShopItem item = items[itemIndex];
        item.purchased = true;

        // Take away the money from the player.
        GameManager.inst.TakeMoney(item.price);

        // Instantiate the item prefab and set properties.
        GameObject itemObj = Instantiate(item.prefabToSpawn);
        itemObj.GetComponent<Item>().SetProperties(itemIndex);

        // Update button UI.
        UpdateItemButton(itemIndex);
    }

    // Called when an item has been destroyed.
    // The player can now buy this again.
    public void OnItemDestroyed (int itemIndex)
    {
        items[itemIndex].purchased = false;
        UpdateItemButton(itemIndex);
    }

    // Called when a new wave has begun.
    // Make sure that the shop is closed.
    void OnBeginWave ()
    {
        if(shopObject.activeInHierarchy)
            ToggleShop();
    }
}