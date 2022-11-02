using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedUI : MonoBehaviour
{
    public UpgradeMenuController controller;
    public List<WeapItemSlotUI> weapUI = new();
    public List<WeapItemSlotUI> accessoryUI = new();
    private List<WeapItemSlotUI> allSlots = new();

    public void Start()
    {
        allSlots.AddRange(weapUI);
        allSlots.AddRange(accessoryUI);
    }

    public void UpdateWeapUI(int idNum, WeaponBase wb)
    {
        weapUI[idNum].weapName = wb.wName;
        weapUI[idNum].weapImg.sprite = wb.shopItemInfo.splashImg;
        weapUI[idNum].lvl.text = wb.level.ToString();

        weapUI[idNum].weapImg.enabled = true;
        weapUI[idNum].weapLvlBadge.SetActive(true);
    }

    public void UpdateAccUI()
    {

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
        foreach(WeapItemSlotUI ws in allSlots)
        {
            if(selectedOne.indentity == ws.indentity)
            {
                //select it
                ws.selectBorder.SetActive(true);
                //populate focusUI
                controller.PopulateFocusUI(ws);
                //set it to focus
            }
            else
            {
                //deselect it
                ws.selectBorder.SetActive(false);
            }
        }
    }
}
