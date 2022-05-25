using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemPrefab : MonoBehaviour
{
    public TextMeshProUGUI title, desc, amount;
    public Button buyButton;
    public Image splash, currency;
    [Space(10)]
    public int indexNum;

    public void UpdatePrefab(string tit, string des, string amt, Image sp, Image cur, int ind)
    {
        title.text = tit;
        desc.text = des;
        amount.text = amt;
        splash = sp;
        currency = cur;
        indexNum = ind;
    }
    public void UpdatePrefab(ShopItemSO shopItem)
    {

        title.text = shopItem.weapName;
        desc.text = shopItem.description;
        splash = shopItem.splashImg;
        currency = shopItem.currency;
        indexNum = shopItem.indexNum;
    }
}
