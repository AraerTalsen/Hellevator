using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Market : MonoBehaviour
{
    public GameObject statBoost, powerUps, consumables;
    public Button[] itemPurchase, powerPurchase, statPurchase;
    private Possessions pos;
    private string selectedShop;
    private LaserActivation la;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMarket()
    {
        pos = FindObjectOfType<Possessions>();
        la = GetComponent<LaserActivation>();
        gameObject.SetActive(true);
        selectedShop = EventSystem.current.currentSelectedGameObject.name;
        ChooseShop(selectedShop, true);
        ItemCapacity();       
    }

    public void CloseMarket()
    {
        ChooseShop(selectedShop, false);
        gameObject.SetActive(false);
        ItemCapacity();
    }

    private void ItemCapacity()
    {
        switch (selectedShop)
        {
            case "Shop":
                {
                    if (pos.CheckInventoryFull())
                    {
                        for (int i = 0; i < itemPurchase.Length; i++) itemPurchase[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        for (int i = 0; i < itemPurchase.Length; i++) itemPurchase[i].gameObject.SetActive(true);
                    }
                    break;
                }
            case "Upgrades":
                {
                    bool[] checkPowers = pos.GetPowerups();
                    for (int i = 0; i < powerPurchase.Length; i++)
                    {
                        if (checkPowers[i]) powerPurchase[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case "Stats":
                {
                    bool[] checkStats = pos.GetStatsIsCap();
                    for (int i = 0; i < statPurchase.Length; i++)
                    {
                        if (checkStats[i]) statPurchase[i].gameObject.SetActive(false);
                    }
                    break;
                }
        }
    }

    private void ChooseShop(string selectedShop, bool state)
    {
        switch (selectedShop)
        {
            case "Shop":
                {
                    consumables.gameObject.SetActive(state);
                    break;
                }
            case "Upgrades":
                {
                    powerUps.gameObject.SetActive(state);
                    break;
                }
            case "Stats":
                {
                    statBoost.gameObject.SetActive(state);
                    break;
                }
        }
    }

    public void Purchase()
    {       
        string purchase = EventSystem.current.currentSelectedGameObject.name;
        string priceTag = EventSystem.current.currentSelectedGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        int price;
        int.TryParse(priceTag, out price);
        Coins.AddSubtractCoins(-price);
        switch (selectedShop)
        {
            case "Shop":
                {                
                    pos.AddToInventory(purchase);
                    break;
                }
            case "Upgrades":
                {
                    pos.PurchasePowerup(purchase);
                    la.SetButtonActive(purchase);
                    break;
                }
            case "Stats":
                {
                    float a;
                    string amount = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).name;
                    float.TryParse(amount, out a);
                    pos.IncreaseStat(purchase, a);
                    break;
                }
        }
        ItemCapacity();
    }
}
