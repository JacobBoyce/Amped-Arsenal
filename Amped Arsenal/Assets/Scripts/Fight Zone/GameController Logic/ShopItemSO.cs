using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "###ShopItem", menuName = "Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string weapName, description, amount;
    public Sprite splashImg, currency;
    public int indexNum;
}
