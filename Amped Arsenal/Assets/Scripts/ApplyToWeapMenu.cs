using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApplyToWeapMenu : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image relicImage;
    public GameObject weaponChoicePrefab, weapChoicePrefabContainer;
    [Header("Selected Weapon")]
    public WeapChoicePrefab selectedWeap;
    public RelicBase relicToApply;

    public PlayerController player;

    public List<GameObject> weapOptions = new();

    public void PopulateWeapChoiceList(PlayerController p1, RelicBase relic)
    {
        titleText.text = "Choose a Weapon to apply the " + relic.relicName + " Weapon Mod";
        relicImage.sprite = relic.relicImg;
        player = p1;
        relicToApply = relic;
        ClearOptions();

        int i = 0;
        foreach(GameObject weapObj in player.equippedWeapons)
        {
            WeaponBase tempWeap = weapObj.GetComponent<WeaponBase>();
            WeapChoicePrefab choicePrefab = weapOptions[i].GetComponent<WeapChoicePrefab>();

                choicePrefab.weapName.text = tempWeap.wName;
                choicePrefab.weapImg.sprite = tempWeap.shopItemInfo.splashImg;
                choicePrefab.weapImg.gameObject.SetActive(true);

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
                if(wcp.isSelected != true)
                {
                    //select it
                    wcp.Select();
                    selectedWeap = wcp;
                }
            }
            else
            {
                //deselect it
                wcp.Deselect();
                
            }
        }
    }

    //Apply Button calls this
    public void ApplySelectedWeapon()
    {
        relicToApply.ApplyRelic(player, selectedWeap.weapName.text);
    }

}
