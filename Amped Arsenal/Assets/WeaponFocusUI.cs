using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponFocusUI : MonoBehaviour
{
    public Image upgradeWeaponImg;
    public Button upgradeButton;
    public TextMeshProUGUI weapName, lvlUpFrom, lvlUpTo, costUI;

    public GameObject levelContainer, upgradeSlotParent, statsUpSlotPrefab;
    private StatUpInfoSlot tempSlot;
    private WeaponBase focusedWeap;
    private PlayerController player;

    public List<GameObject> upInfoSlot = new();

    public void UpdateFocusUI(WeaponBase wb, PlayerController p1)
    {
        focusedWeap = wb;
        player = p1;

        upgradeWeaponImg.sprite = wb.shopItemInfo.splashImg;
        upgradeWeaponImg.enabled = true;

        weapName.text = wb.wName;

        levelContainer.SetActive(true);
        lvlUpFrom.text = wb.level.ToString();
        lvlUpTo.text = (wb.level + 1).ToString();
        costUI.text = "Cost: " + wb.weapUpgrades.costValues[wb.level-1];


        //create a loop foreach of wb's upgrade stats from
        foreach(WeapUpgrade up in wb.weapUpgrades.UpgradeList)
        {
            GameObject tempSlotPrefab = Instantiate(statsUpSlotPrefab);
            tempSlotPrefab.transform.SetParent(upgradeSlotParent.transform);
            tempSlot = tempSlotPrefab.GetComponent<StatUpInfoSlot>();
            //set image here

            tempSlot.upFrom.text = up.upValues[wb.level-1].ToString();
            tempSlot.upTo.text = up.upValues[wb.level].ToString();

            
            //add to upgrade list
            upInfoSlot.Add(tempSlotPrefab);
        }
        upgradeSlotParent.SetActive(true);

        //check if upgrade button should be active
        //Debug.Log("Cost of upgrade: " + wb.weapUpgrades.costValues[wb.level - 1] + "  >= current xp: " + player._stats["xp"].Value);
        if (wb.weapUpgrades.costValues[wb.level-1] <= player._stats["xp"].Value)
        {
            //Debug.Log("turnon on init");
            upgradeButton.interactable = true;
        }
        else
        {
            //Debug.Log("turnoff on init");
            upgradeButton.interactable = false;
            //costUI.text = "Cost: ";
        }
    }

    public void ClearFocusUI()
    {
        lvlUpFrom.text = "";
        lvlUpTo.text = "";
        upgradeWeaponImg.enabled = false;
        levelContainer.SetActive(true);
        weapName.text = "";
        //disable upgrade button
        //Debug.Log("turnoff on focus off");
        upgradeButton.interactable = false;
        costUI.text = "Cost: ";

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
                //upgrade weapon
                tempWeap.UpgradeWeapon();

                //update focused weapon
                ClearFocusUI();
                UpdateFocusUI(tempWeap, player);
                player.RemoveXP(tempWeap.weapUpgrades.costValues[tempWeap.level-1]);
            }
        }
    }
}
