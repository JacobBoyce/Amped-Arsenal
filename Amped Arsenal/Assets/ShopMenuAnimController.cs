using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuAnimController : MonoBehaviour
{
    public GameZoneController cont;
    public void TurnOffShopUI()
    {
        cont.TurnOffShop();
    }
}
