using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour
{
    public WeaponFocusUI weapFocus;
    public EquippedUI equippedUICont;
    public GameObject blankSlotPrefab;
    private PlayerController playerCont;

    public void Awake()
    {
        playerCont = PlayerController.playerObj;
        //playerCont.equippedWeapons
    }

    public void PopulateUI()
    {
        //update all the slots with the current equipped weapons and artifacts

        //clear the list so its empty then re populate the list.

        equippedUICont.ClearSlots();
        weapFocus.ClearFocusUI();


        //check the slot first to see if the thing we are about to populate is already in the list
        if (playerCont.equippedWeapons.Count != 0)
        {
            int slotNum = 0;
            foreach(GameObject go in playerCont.equippedWeapons)
            {
                //populate ui
                WeaponBase tempWeap = go.GetComponent<WeaponBase>();
                equippedUICont.UpdateWeapUI(slotNum, tempWeap);
                slotNum++;
            }
        }
    }

    public void PopulateFocusUI(WeapItemSlotUI ws)
    {
        //go off the slots weapon name to get the weapon controller from the players equipped list with findweapon method
        foreach (GameObject go in playerCont.equippedWeapons)
        {
            if(go.GetComponent<WeaponBase>().wName.Equals(ws.weapName))
            {
                WeaponBase tempWeap = go.GetComponent<WeaponBase>();

                weapFocus.UpdateFocusUI(tempWeap,playerCont);
            }
        }

    }
}
