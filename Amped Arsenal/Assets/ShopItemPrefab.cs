using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemPrefab : MonoBehaviour
{
    public string weapName = "None";
    public Button buyButton;
    public Image splash, currency;
    [Space(10)]
    public int indexNum;

    public void UpdatePrefab(string wpName, Image sp, Image cur, int ind)
    {
        weapName = wpName;
        splash = sp;
        currency = cur;
        indexNum = ind;
    }
    public void UpdatePrefab(ShopItemSO shopItem)
    {
        weapName = shopItem.weapName;
        splash.sprite = shopItem.splashImg;
        currency.sprite = shopItem.currency;
        indexNum = shopItem.indexNum;
    }

    public void UpdatePrice(int newPrice)
    {
        buyButton.GetComponentInChildren<TextMeshProUGUI>().text = newPrice.ToString();
    }
}
