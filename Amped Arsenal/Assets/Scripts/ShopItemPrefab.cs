using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemPrefab : MonoBehaviour
{
    public string weapName = "None";
    public Button buyButton;
    public TextMeshProUGUI priceUI;
    public Image splash;
    [Space(10)]
    public int indexNum;


    public void UpdatePrefab(string wpName, Image sp, int ind)
    {
        weapName = wpName;
        splash = sp;
        indexNum = ind;
    }
    public void UpdatePrefab(ShopItemSO shopItem)
    {
        weapName = shopItem.weapName;
        splash.sprite = shopItem.splashImg;
        indexNum = shopItem.indexNum;
    }

    public void UpdatePrice(int newPrice)
    {
        priceUI.text = newPrice.ToString();
    }
    public void UpdatePrice(string cantBuy)
    {
        priceUI.text = cantBuy;
    }
}
