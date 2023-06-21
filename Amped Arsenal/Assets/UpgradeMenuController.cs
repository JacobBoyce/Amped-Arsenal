using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenuController : MonoBehaviour,IEnumerable<UpgradeIcons>
{
    public WeaponFocusUI weapFocus;
    public EquippedUI equippedUICont;
    //public GameObject blankSlotPrefab;
    public TextMeshProUGUI xpText;
    private PlayerController playerCont;

    public List<UpgradeIcons> upIcons = new List<UpgradeIcons>();
    UpgradeIcons Find(WeapUpgrade.WeaponUpgrade wuType) => upIcons.Find(x => x.upType.Equals(wuType));// FirstOrDefault(x => x.Name.Equals(type, StringComparison.OrdinalIgnoreCase));


    public Sprite GetUpIcon(WeapUpgrade.WeaponUpgrade wuType)
    {
        foreach(UpgradeIcons icon in upIcons)
        {
            if(icon.upType.Equals(wuType))
            {
                return icon.upIcon;
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

        xpText.text = "XP: " + playerCont._stats["xp"].Value;

        //check the slot first to see if the thing we are about to populate is already in the list
        UpdateSlotUIInfo();
    }

    public void UpdateSlotUIInfo()
    {
        if (playerCont.equippedWeapons.Count != 0)
        {
            int slotNum = 0;
            foreach (GameObject go in playerCont.equippedWeapons)
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

[System.Serializable]
 public class UpgradeIcons
 {
    [SerializeField]
        public WeapUpgrade.WeaponUpgrade upType;
        [SerializeField]
        public Sprite upIcon;

        
 }
