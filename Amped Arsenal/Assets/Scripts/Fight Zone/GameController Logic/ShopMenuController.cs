using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopMenuController : MonoBehaviour
{
    public GameZoneController controller;
    public WeaponLib weapLib;
    public GameObject shopWeaponParent, itemPrefab, tempItemPrefab;
    public List<ShopItemPrefab> buyableItems;
    GameObject tempItemObj;
    public int amountBought, price = 25;
    public bool populatedShop = false, foundNum;
    int[] weapChoices = new int[3]{-1,-1,-1};
    int v1;
    //player controller.p1
    
    public void TurnOffShopUI()
    {
        controller.TurnOffShop();
    }

    public void InitShop()
    {
        if(populatedShop == false)
        {
            MakeShopItems();
            populatedShop = true;
        }
    }

    public void ReRollShop()
    {
        //take money away
        //delete current shop items
        foreach(ShopItemPrefab pf in buyableItems)
        {
            Destroy(pf.gameObject);
        }
        buyableItems.Clear();
        weapChoices[0] = -1;
        weapChoices[1] = -1;
        weapChoices[2] = -1;
        foundNum = false;
        MakeShopItems();
    }

    public void ChooseWeapons()
    {
        v1 = Random.Range(0,weapLib.weaponList.Count);
        while(controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == true)
        {
            v1 = Random.Range(0,weapLib.weaponList.Count);
        }
        weapChoices[0] = v1;
    
        for (int i = 1; i < weapChoices.Length; i++)
        {
            v1 = Random.Range(0,weapLib.weaponList.Count);
            if(controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == false)
            {
                for (int j = 0; j < i; j++)
                {
                    while (v1 == weapChoices[j])
                    {
                        v1 = Random.Range(0,weapLib.weaponList.Count);
                        while(controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == true)
                        {
                            v1 = Random.Range(0,weapLib.weaponList.Count);
                        }
                        j = 0;
                    }
                }
                weapChoices[i] = v1;
            }
            else
            {
                i--;
            }
        }
    }
    

    public void MakeShopItems()
    {
        ChooseWeapons();

        for(int i = 0; i < 3; i++)
        {
            tempItemPrefab = Instantiate(itemPrefab);
            tempItemPrefab.transform.SetParent(shopWeaponParent.transform);
            tempItemPrefab.transform.localScale = new Vector3(1,1,1);

            tempItemPrefab.GetComponent<ShopItemPrefab>().UpdatePrefab(weapLib.weaponList[weapChoices[i]].GetComponent<WeaponBase>().shopItemInfo);
            //set what buttons do
            int tempInt = i;
            tempItemPrefab.GetComponent<ShopItemPrefab>().buyButton.onClick.AddListener(() => ButtonTask(tempInt));
            //set prices
            tempItemPrefab.GetComponent<ShopItemPrefab>().amount.text = price.ToString();
            buyableItems.Add(tempItemPrefab.GetComponent<ShopItemPrefab>());
            Debug.Log(i);
        }
        CheckIfCanBuy();
    }

    public void ButtonTask(int index)
    {
        controller.p1._stats["gold"].Value -= price;
        //if index == 0
        //buyableweapons[index] has been bought
        Debug.Log(index);
        controller.p1.AddWeaponToCache(buyableItems[index].title.text);
        ScalePrices();
        CheckIfCanBuy();
    }

    public void CheckIfCanBuy()
    {
        foreach(ShopItemPrefab sp in buyableItems)
        {
            if(price > controller.p1._stats["gold"].Value)
            {
                sp.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "X";
                sp.buyButton.enabled = false;
            }
            else
            {
                sp.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
                sp.buyButton.enabled = true;
            }
        }
    }

    public void ScalePrices()
    {
        amountBought++;
        if(amountBought == 1)
        {
            price = 50;
        }
    }
}
