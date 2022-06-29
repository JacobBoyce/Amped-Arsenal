using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedUI : MonoBehaviour
{
    public List<WeapItemSlotUI> weapUI = new();
    public List<WeapItemSlotUI> accessoryUI = new();
    private List<WeapItemSlotUI> allSlots = new();

    public void Start()
    {
        allSlots.AddRange(weapUI);
        allSlots.AddRange(accessoryUI);
    }

    public void UpdateWeapUI()
    {

    }

    public void UpdateAccUI()
    {

    }

    public void SelectThisOne(WeapItemSlotUI selectedOne)
    { 
        foreach(WeapItemSlotUI ws in allSlots)
        {
            if(selectedOne.indentity == ws.indentity)
            {
                //select it
                ws.selectBorder.SetActive(true);
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
