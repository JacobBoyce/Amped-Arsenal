using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponFocusUI : MonoBehaviour
{
    public Image upgradeWeaponImg;
    public TextMeshProUGUI weapName, lvlUpFrom, lvlUpTo;

    public GameObject levelContainer, upgradeSlotParent, statsUpSlotPrefab;
    private StatUpInfoSlot tempSlot;

    public List<GameObject> upInfoSlot = new();

    public void UpdateFocusUI(WeaponBase wb)
    {
        upgradeWeaponImg.sprite = wb.shopItemInfo.splashImg;
        upgradeWeaponImg.enabled = true;

        weapName.text = wb.wName;

        levelContainer.SetActive(true);
        lvlUpFrom.text = wb.level.ToString();
        lvlUpTo.text = (wb.level + 1).ToString();


        //create a loop foreach of wb's upgrade stats from
        foreach(WeapUpgrade up in wb.weapUpgrades.UpgradeList)
        {
            GameObject tempSlotPrefab = Instantiate(statsUpSlotPrefab);
            tempSlotPrefab.transform.parent = upgradeSlotParent.transform;
            tempSlot = tempSlotPrefab.GetComponent<StatUpInfoSlot>();
            //set image here

            tempSlot.upFrom.text = up.upValues[wb.level-1].ToString();
            tempSlot.upTo.text = up.upValues[wb.level].ToString();

            
            //add to upgrade list
            upInfoSlot.Add(tempSlotPrefab);
        }

        upgradeSlotParent.SetActive(true);

    }

    public void ClearFocusUI()
    {
        upgradeWeaponImg.enabled = false;
        levelContainer.SetActive(true);
        weapName.text = "";

        upInfoSlot.Clear();
    }
}
