using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedUI : MonoBehaviour
{
    public UpgradeMenuController controller;
    public List<WeapItemSlotUI> weapUI = new();
    private List<WeapItemSlotUI> allSlots = new();
    public Color maxLvlColor;

    public void Start()
    {
        allSlots.AddRange(weapUI);
    }

    public void UpdateWeapUI(int idNum, WeaponBase wb, bool canUpgrade)
    {
        weapUI[idNum].weapName = wb.wName;
        weapUI[idNum].weapImg.sprite = wb.shopItemInfo.splashImg;
        weapUI[idNum].weapImg.enabled = true;
        //weapUI[idNum].lvl.text = wb.level.ToString();
        //weapUI[idNum].lvlBar.FillAmountTweenAtSpeed(wb.level/5, 2 * Time.deltaTime);
        weapUI[idNum].lvlBar.fillAmount = wb.level / 5f;
        if(weapUI[idNum].lvlBar.fillAmount == 1)
        {
            weapUI[idNum].lvlBar.color = maxLvlColor;
        }
        weapUI[idNum].GetComponent<Button>().interactable = true;

        weapUI[idNum].UpgradeableCheck(canUpgrade);
    }

    

    public void ClearSlots()
    {
        foreach(WeapItemSlotUI ws in weapUI)
        {
            ws.ClearStuff();
        }
    }

    public void SelectThisOne(WeapItemSlotUI selectedOne)
    { 
        //check if its already selected if so do nothing

        foreach(WeapItemSlotUI ws in allSlots)
        {
            if(selectedOne.indentity == ws.indentity)
            {
                //if(!ws.isSelected)
                //{
                    //select it
                    ws.Select();
                    controller.weapFocus.ClearFocusUI();
                    //populate focusUI
                    controller.PopulateFocusUI(ws);
                    //set it to focus
                //}
            }
            else
            {
                //deselect it
                ws.Deselect();                
            }
        }
    }
}
