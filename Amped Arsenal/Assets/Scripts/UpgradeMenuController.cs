using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeMenuController : MonoBehaviour,IEnumerable<UpgradeIcons>
{
    public WeaponFocusUI weapFocus;
    public MenuItemSelectionManager menuController;
    public EquippedUI equippedUICont;
    //public GameObject blankSlotPrefab;
    public TextMeshProUGUI xpText;
    private PlayerController playerCont;


    public List<UpgradeIcons> upIcons = new();
    UpgradeIcons Find(WeapUpgrade.WeaponUpgrade wuType) => upIcons.Find(x => x.upType.Equals(wuType));// FirstOrDefault(x => x.Name.Equals(type, StringComparison.OrdinalIgnoreCase));


    public UpgradeIcons GetUpIcon(WeapUpgrade.WeaponUpgrade wuType)
    {
        foreach(UpgradeIcons icon in upIcons)
        {
            if(icon.upType.Equals(wuType))
            {
                return icon;
            }
        }

        return null;
    }

    public IEnumerator<UpgradeIcons> GetEnumerator() => upIcons.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

        xpText.text = playerCont._stats["xp"].Value.ToString();

        //check the slot first to see if the thing we are about to populate is already in the list
        UpdateSlotUIInfo(true);
    }

    public void SelectBuyButton()
    {
        StartCoroutine(menuController.SetSelectedAfterOneFrame(6));
    }

    public void UpdateSlotUIInfo(bool isFirstLoad)
    {
        if (playerCont.equippedWeapons.Count != 0)
        {
            int slotNum = 0;
            foreach (GameObject go in playerCont.equippedWeapons)
            {
                //populate ui
                WeaponBase tempWeap = go.GetComponent<WeaponBase>();
                
                bool canUpgrade = false;

                //check if upgradeable
                if (tempWeap != null && !tempWeap.IsMaxLvl())
                {
                    if (tempWeap.weapUpgrades.costValues[tempWeap.level - 1] <= playerCont._stats["xp"].Value)
                    {
                        // can be upgraded
                        canUpgrade = true;
                    }
                }
                if(tempWeap.IsMaxLvl())
                {
                    //set to "deselected"
                    canUpgrade = false;
                }

                equippedUICont.UpdateWeapUI(slotNum, tempWeap, canUpgrade);
                
                slotNum++;
            }
        }
        if(isFirstLoad)
        {
            equippedUICont.SelectThisOne(menuController.menuItems[0].GetComponent<WeapItemSlotUI>()); 
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

    public void CheckUpgradeableWeaps()
    {
        foreach(GameObject go in playerCont.equippedWeapons)
        {
            WeaponBase weap = go.GetComponent<WeaponBase>();
            if (weap != null && !weap.IsMaxLvl())
            {
                if (weap.weapUpgrades.costValues[weap.level - 1] <= playerCont._stats["xp"].Value)
                {
                    // can be upgraded
                }
            }
            if(weap.IsMaxLvl())
            {
                //set to "deselected"
            }
        }
    }
}

[System.Serializable]
 public class UpgradeIcons
 {
    [SerializeField]
        public WeapUpgrade.WeaponUpgrade upType;
        [SerializeField]
        public Sprite upIcon;
        public int MaxValue;        
 }
