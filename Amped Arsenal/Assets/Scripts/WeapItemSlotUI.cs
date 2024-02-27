using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeapItemSlotUI : MonoBehaviour
{
    public int indentity;
    public string weapName;
    public GameObject selectBorder;
    public Image weapUIObj;
    [SerializeField]
    private Sprite deselected, upgradable, selected, upgradableANDselected;
    public bool isSelected = false, isUpgradeable = false;
    public Image weapImg, lvlBar;
    public GameObject weapLvlBadge;
    public TextMeshProUGUI lvl;


    public void ClearStuff()
    {
        weapName = "";
        //selectBorder.SetActive(false);
        Deselect();
        weapImg.enabled = false;
        lvl.text = "";
        lvlBar.fillAmount = 0;
    }

    public void Deselect()
    {
        isSelected = false;

        if(!isUpgradeable)
        {
            weapUIObj.sprite = deselected;
        }
        else
        {
            weapUIObj.sprite = upgradable;
        }
    }

    public void Select()
    {
        isSelected = true;

        if(!isUpgradeable)
        {
            weapUIObj.sprite = selected;
        }
        else
        {
            weapUIObj.sprite = upgradableANDselected;
        }
    }

    public void UpgradeableCheck(bool canUpgrade)
    {
        isUpgradeable = canUpgrade;
        if(isSelected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }
}
