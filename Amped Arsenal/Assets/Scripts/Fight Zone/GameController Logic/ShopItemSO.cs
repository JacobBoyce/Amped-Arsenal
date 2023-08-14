using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "###ShopItem", menuName = "Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string weapName;
    public Sprite splashImg;
    public int indexNum;
}
