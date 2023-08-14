using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyToWeapMenu : MonoBehaviour
{
    public GameObject weaponChoicePrefab, weapChoicePrefabContainer;
    [Header("Selected Weapon")]
    public WeapChoicePrefab selectedWeap;
    public RelicBase relicToApply;

    public PlayerController player;

    public List<GameObject> weapOptions = new List<GameObject>();

    public void PopulateWeapChoiceList(PlayerController p1, RelicBase relic)
    {
        player = p1;
        relicToApply = relic;
        ClearOptions();

        int i = 0;
        foreach(GameObject weapObj in player.equippedWeapons)
        {
            WeaponBase tempWeap = weapObj.GetComponent<WeaponBase>();

                weapOptions[i].GetComponent<WeapChoicePrefab>().weapName.text = tempWeap.wName;
                weapOptions[i].GetComponent<WeapChoicePrefab>().weapImg.sprite = tempWeap.shopItemInfo.splashImg;
                weapOptions[i].GetComponent<WeapChoicePrefab>().weapImg.gameObject.SetActive(true);

            if(tempWeap.currentEquippedSlots < tempWeap.maxSlots)
            {
                //GameObject tempWeapChoicePrefab = Instantiate(weaponChoicePrefab);
                //tempWeapChoicePrefab.transform.SetParent(weapChoicePrefabContainer.transform);
 
                
                weapOptions[i].GetComponent<Button>().interactable = true;
                i++;
                //weapOptions.Add(tempWeapChoicePrefab);
            }
        }
    }

    public void ClearOptions()
    {
        foreach(GameObject go in weapOptions)
        {
            //make the slots empty
            go.GetComponent<WeapChoicePrefab>().weapName.text = "";
            go.GetComponent<WeapChoicePrefab>().weapImg.gameObject.SetActive(false);
            go.GetComponent<Button>().interactable = false;
        }
    }

    //each weapon slot calls this method
    public void ApplyWeaponEffect(WeapChoicePrefab weapChoice)
    {
        //check if its already selected if so do nothing

        foreach(GameObject ws in weapOptions)
        {
            WeapChoicePrefab wcp = ws.GetComponent<WeapChoicePrefab>();
            if(weapChoice.weapName.Equals(wcp.weapName) == true)
            {
                if(wcp.selectionBorder.activeSelf != true)
                {
                    //select it
                    wcp.selectionBorder.SetActive(true);
                    selectedWeap = wcp;
                }
            }
            else
            {
                //deselect it
                wcp.selectionBorder.SetActive(false);
                
            }
        }
    }

    //Apply Button calls this
    public void ApplySelectedWeapon()
    {
        relicToApply.ApplyRelic(player, selectedWeap.weapName.text);
    }

}
