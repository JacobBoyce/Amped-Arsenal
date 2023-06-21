using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
    public GameZoneController controller;
    public WeaponLib weapLib;
    public GameObject shopWeaponParent, itemPrefab, tempItemPrefab, rerollButton;
    public List<ShopItemPrefab> buyableItems;
    public ShopItemPrefab tempShopItem;
    GameObject tempItemObj;
    public int amountBought, price = 25;
    public bool populatedShop = false;
    int[] weapChoices = new int[3]{-1,-1,-1};
    int v1, shopDirty = 0;
    public GameObject uiParent;
    public TextMeshProUGUI goldText;
    public GameObject upgradeButtonUI;
    //player controller.p1

    /*public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //turn on upgrade screen and turn off other ui
            controller.FocusUI(uiParent);
        }
    }*/


    public void TurnOffShopUI()
    {
        controller.TurnOffShop();
    }

    public void InitShop()
    {
        goldText.text = controller.p1._stats["gold"].Value.ToString();
        if (populatedShop == false)
        {
            MakeShopItems();
            populatedShop = true;
        }
        CheckIfCanBuy();
        upgradeButtonUI.SetActive(false);
    }

    public void ReRollShop()
    {
        //take money away-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //delete current shop items
        /*foreach (ShopItemPrefab pf in buyableItems)
        {
            Destroy(pf.gameObject);
        }*/
        //buyableItems.Clear();
        weapChoices[0] = -1;
        weapChoices[1] = -1;
        weapChoices[2] = -1;
        MakeShopItems();
        shopDirty = 0;

        if (NumAvailableWeaponsToBuy() == 3 && shopDirty == 0)
        {
            rerollButton.GetComponent<Button>().interactable = false;
        }
    }

    public void ChooseWeapons()
    {
        int numAvail = NumAvailableWeaponsToBuy();
        //check if there are less than 3 weapons to buy, if there are only 3 choices or less left, disable re roll shop option
        if (numAvail == 1 && numAvail > 0)
        {
            v1 = 0;
            while (controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == true)
            {
                v1++;
            }
            weapChoices[0] = v1;
            rerollButton.GetComponent<Button>().interactable = false;

        }
        else if (numAvail == 2 && numAvail > 0)
        {
            v1 = 0;
            while (controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == true)
            {
                v1++;
            }
            weapChoices[0] = v1;

            bool foundAvail = false;
            v1 = 0;
            while (foundAvail == false)
            {
                if (controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == false)
                {
                    if (v1 != weapChoices[0])
                    {
                        foundAvail = true;
                    }
                    else
                    {
                        v1++;
                    }
                }
                else
                {
                    v1++;
                }
            }
            weapChoices[1] = v1;
            rerollButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            //randomly chooses a weapon from the library
            v1 = Random.Range(0, weapLib.weaponList.Count);
            // checks if we already have it equipped, if we do then keep trying until we get one we dont have
            while (controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == true)
            {
                v1 = Random.Range(0, weapLib.weaponList.Count);
            }
            //set the unequipped weapon to sell in slot 1
            weapChoices[0] = v1;

            //loop to get the other two choices
            for (int i = 1; i < weapChoices.Length; i++)
            {
                //randomly choose another weapon from the list
                v1 = Random.Range(0, weapLib.weaponList.Count);
                //check if its equipped
                if (controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == false)
                {
                    //loop thru the list of choices that have already been set so there are no duplicates
                    for (int j = 0; j < i; j++)
                    {
                        //find one until it isnt one in the shop
                        while (v1 == weapChoices[j])
                        {
                            //find one that also isnt equipped
                            v1 = Random.Range(0, weapLib.weaponList.Count);
                            while (controller.p1.FindWeapon(weapLib.weaponList[v1].GetComponent<WeaponBase>().wName) == true)
                            {
                                v1 = Random.Range(0, weapLib.weaponList.Count);
                            }
                            j = 0;
                        }
                    }
                    //set the found weapon to the next slot
                    weapChoices[i] = v1;
                }
                //if it is equipped then start the for loop over until we get one we havent equipped
                else
                {
                    i--;
                }
            }
        }
    }

    public int NumAvailableWeaponsToBuy()
    {
        int currentlyEquipped = 0;
        foreach(GameObject weap in weapLib.weaponList)
        {
            if (controller.p1.FindWeapon(weap.GetComponent<WeaponBase>().wName) == true)
            {
                currentlyEquipped++;
            }
        }

        currentlyEquipped = weapLib.weaponList.Count - currentlyEquipped;
        return currentlyEquipped;
    }
    

    public void MakeShopItems()
    {
        ChooseWeapons();
        
        for(int i = 0; i < 3; i++)
        {
            //if weapon name is blank (-1) then do none of this other than making the prefab
            
            /*
            tempItemPrefab = Instantiate(itemPrefab);
            tempItemPrefab.transform.SetParent(shopWeaponParent.transform);
            tempItemPrefab.transform.localScale = new Vector3(1,1,1);
            tempShopItem = tempItemPrefab.GetComponent<ShopItemPrefab>();
            */

            tempShopItem = buyableItems[i];

            if (weapChoices[i] != -1)
            {
                tempShopItem.UpdatePrefab(weapLib.weaponList[weapChoices[i]].GetComponent<WeaponBase>().shopItemInfo);

                //set what buttons do
                int tempInt = i;
                tempShopItem.buyButton.onClick.AddListener(() => ButtonTask(tempInt));
                //set prices
                tempShopItem.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
            }
            else if (weapChoices[i] == -1)
            {
                tempShopItem.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "X";
                tempShopItem.buyButton.interactable = false;
            }
        }
        CheckIfCanBuy();
    }

    public void ButtonTask(int index)
    {
        controller.p1.RemoveGold(price); //_stats["gold"].Value -= price;
        goldText.text = controller.p1._stats["gold"].Value.ToString();
        //if index == 0
        //buyableweapons[index] has been bought
        //Debug.Log(index);
        controller.p1.AddWeaponToCache(buyableItems[index].weapName);
        ScalePrices();
        CheckIfCanBuy();
        shopDirty += 1;
        int numLeft = NumAvailableWeaponsToBuy();
        //if dirty = 3 and num left is > 0 allow reroll and fill the blanks
        //if dirty = 2 and num left is > 2 allow reroll and fill in blanks
        //if dirty = 1 and num left is >= 3 allow reroll
        //if dirty = 0 and num left is >= 3 allow reroll
        if(shopDirty == 3 && numLeft > 0)
        {
            rerollButton.GetComponent<Button>().interactable = true;
        }
        else if(shopDirty == 2 && numLeft >= 2)
        {
            rerollButton.GetComponent<Button>().interactable = true;
        }
        else if(shopDirty == 1 && numLeft >= 3)
        {
            rerollButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            rerollButton.GetComponent<Button>().interactable = false;
        }
    }

    public void CheckIfCanBuy()
    {
        //##############################################################################################################
        //make sure if the buyable item is a blank one that it cant be activated
        foreach(ShopItemPrefab sp in buyableItems)
        {
            //check if you have enough gold to buy
            if(price > controller.p1._stats["gold"].Value)
            {
                sp.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
                sp.buyButton.enabled = false;
                sp.buyButton.interactable = false;
            }
            else
            {
                sp.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
                sp.buyButton.enabled = true;
                sp.buyButton.interactable = true;
            }

            //check if it has already been bought
            if (controller.p1.FindWeapon(sp.weapName) || sp.weapName == "None")
            {
                sp.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "X";
                sp.buyButton.enabled = false;
                sp.buyButton.interactable = false;
            }
        }
    }

    public void ScalePrices()
    {
        amountBought++;
        price += 25;
        if (amountBought == 1)
        {
            
        }

        foreach(ShopItemPrefab sp in buyableItems)
        {
            sp.UpdatePrice(price);
        }
    }
}
