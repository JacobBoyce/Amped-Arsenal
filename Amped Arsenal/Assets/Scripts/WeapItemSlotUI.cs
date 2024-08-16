using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeapItemSlotUI : MonoBehaviour
{
    public EquippedUI eUIController;
    
    [Space(15)]
    public int indentity;
    public string weapName;
    public bool isSelected = false, isUpgradeable = false;
    public Image weapImg, lvlBar;
    public TextMeshProUGUI lvl;

    void Awake()
    {
        GetComponent<UIInteracableObjectVisuals>().OnObjectHovered += Hovered;
    }
    void OnDestroy()
    {
        GetComponent<UIInteracableObjectVisuals>().OnObjectHovered -= Hovered;
    }

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
            //weapUIObj.sprite = deselected;
            GetComponent<UIInteracableObjectVisuals>().SetNormal(false);
        }
        else
        {
            //weapUIObj.sprite = upgradable;
            GetComponent<UIInteracableObjectVisuals>().SetGreen(false);
        }
    }
    public void Hovered()
    {
        eUIController.SelectThisOne(gameObject.GetComponent<WeapItemSlotUI>());
    }

    //
    public void Select()
    {
        isSelected = true;

        if(!isUpgradeable)
        {
            //weapUIObj.sprite = selected;
            GetComponent<UIInteracableObjectVisuals>().SetNormal(true);
        }
        else
        {
            //weapUIObj.sprite = upgradableANDselected;
            GetComponent<UIInteracableObjectVisuals>().SetGreen(true);
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
