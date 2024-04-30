using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class WeaponFocusUI : MonoBehaviour
{
    
    public Button upgradeButton;
    public Image buttonImage;
    public Image upgradeWeaponImg, modImg;
    public TextMeshProUGUI weapName, weapLvl, modName, costUI;
    public Sprite cantBuyImg, canBuyImg;

    public UpgradeMenuController controller;
    public GameObject levelContainer, upgradeSlotParent, statsUpSlotPrefab;
    private StatUpInfoSlot tempSlot;
    private WeaponBase focusedWeap;
    private PlayerController player;

    public List<GameObject> upInfoSlot = new();

    public void UpdateFocusUI(WeaponBase wb, PlayerController p1)
    {
        //saving passed variables
        focusedWeap = wb;
        player = p1;

        //set weapon image
        upgradeWeaponImg.sprite = focusedWeap.shopItemInfo.splashImg;
        upgradeWeaponImg.enabled = true;
        modImg.enabled = false;
        modName.enabled = false;
        //set weapon name
        weapName.text = focusedWeap.wName;
        weapLvl.text = focusedWeap.level.ToString();

        if(focusedWeap.effectSlots.Count > 0)
        {
            modImg.sprite = focusedWeap.effectSlots[0].GetComponent<EffectBase>().modRelicImg;
            modName.text = focusedWeap.effectSlots[0].GetComponent<EffectBase>().effectName;
            modImg.enabled = true;
            modName.enabled = true;
        }
        

        //show stats of weapon
        //levelContainer.SetActive(true);
        //lvlUpFrom.text = focusedWeap.level.ToString();

        if(focusedWeap.IsMaxLvl())
        {
            //show level is the same
            //lvlUpTo.text = (focusedWeap.level).ToString();
            //disable cost of upgrade button
            upgradeButton.interactable = false;
            // ------- set cost string to  empty -------- 
            costUI.text = "";
        }
        else
        {
            //show next level
            //lvlUpTo.text = (focusedWeap.level + 1).ToString();
            //show cost of upgrade
            costUI.text = ((byte)focusedWeap.weapUpgrades.costValues[focusedWeap.level-1]).ToString();
        }        


        // CREATES LEVEL UP BARS THAT SHOWS THE WEAPON STATS
        for(int i = 0; i < focusedWeap.weapUpgrades.UpgradeList.Count; i ++)
        {
            GameObject tempSlotPrefab = Instantiate(statsUpSlotPrefab);
            tempSlotPrefab.transform.SetParent(upgradeSlotParent.transform);
            tempSlotPrefab.GetComponent<RectTransform>().localScale = new UnityEngine.Vector3(1f,1f,1f);
            
            upInfoSlot.Add(tempSlotPrefab);
        }

        int slotIndex = 0;
        //create a loop foreach of focusedWeap's upgrade stats from
        foreach(WeapUpgrade up in focusedWeap.weapUpgrades.UpgradeList)
        {
            //Debug.Log(up.weapUpType.ToString());
            
            tempSlot = upInfoSlot[slotIndex].GetComponent<StatUpInfoSlot>();
            //add to upgrade list
            
            //set image here
            UpgradeIcons tempUpgradeType = controller.GetUpIcon(up.weapUpType);
            tempSlot.upgradeName = tempUpgradeType.upType.ToString();

            if (focusedWeap.IsMaxLvl())
            {
                //tempSlot.upFrom.text = up.upValues[focusedWeap.level - 1].ToString();
                //tempSlot.upTo.text = up.upValues[focusedWeap.level - 1].ToString();
                if(up.upValues[up.upValues.Length-1] < up.upValues[0])
                {
                    tempSlot.SetFillAmount(0, up.upValues[focusedWeap.level-1],up.upValues[focusedWeap.level-1], tempUpgradeType.MaxValue);
                }
                else
                {
                    tempSlot.SetFillAmount(0, up.upValues[focusedWeap.level-1],up.upValues[focusedWeap.level-1], tempUpgradeType.MaxValue);
                }
            }
            else
            {
                //check if value values goes down (cooldown lowers)
                if(up.upValues[up.upValues.Length-1] < up.upValues[0])
                {
                    tempSlot.SetFillAmount(0, up.upValues[focusedWeap.level - 1],up.upValues[focusedWeap.level], tempUpgradeType.MaxValue);
                }
                else
                {
                    tempSlot.SetFillAmount(0, up.upValues[focusedWeap.level - 1],up.upValues[focusedWeap.level], tempUpgradeType.MaxValue);
                }
                
                //tempSlot.upFrom.text = up.upValues[focusedWeap.level - 1].ToString();
                //tempSlot.upTo.text = up.upValues[focusedWeap.level].ToString();
            }
            slotIndex++;
        }
        upgradeSlotParent.SetActive(true);

        //check if upgrade button should be active
        //Debug.Log("Cost of upgrade: " + focusedWeap.weapUpgrades.costValues[focusedWeap.level - 1] + "  >= current xp: " + player._stats["xp"].Value);
        if(!focusedWeap.IsMaxLvl())
        {
            if (focusedWeap.weapUpgrades.costValues[focusedWeap.level - 1] <= player._stats["xp"].Value)
            {
                //Debug.Log("turnon on init");
                upgradeButton.interactable = true;
                buttonImage.sprite = canBuyImg;
                // make button green
            }
            else
            {
                //Debug.Log("turnoff on init");
                upgradeButton.interactable = false;
                buttonImage.sprite = cantBuyImg;
                // make button red
                //costUI.text = "Cost: ";
            }
        }        
    }

    public void ClearFocusUI()
    {

        upgradeWeaponImg.enabled = false;
        modImg.enabled = false;

        weapName.text = "";
        weapLvl.text = "";
        modName.text = "";

        upgradeButton.interactable = false;
        costUI.text = "";

        foreach (GameObject go in upInfoSlot)
        {
            Destroy(go);
        }
        upInfoSlot.Clear();
    }

    public void UpgradeWeapon()
    {
        //find the weapon thats equipped on the player
        foreach (GameObject go in player.equippedWeapons)
        {
            WeaponBase tempWeap = go.GetComponent<WeaponBase>();
            if (tempWeap.wName.Equals(focusedWeap.wName))
            {
                player.RemoveXP(tempWeap.weapUpgrades.costValues[tempWeap.level - 1]);
                //upgrade weapon
                tempWeap.UpgradeWeapon();

                //update focused weapon
                ClearFocusUI();
                UpdateFocusUI(tempWeap, player);
                controller.UpdateSlotUIInfo();
                
                controller.xpText.text = player._stats["xp"].Value.ToString();
            }
        }
    }
}
