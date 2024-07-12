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
            if(tempWeap.currentEquippedSlots != tempWeap.maxSlots)
            {
                WeapChoicePrefab choicePrefab = weapOptions[i].GetComponent<WeapChoicePrefab>();

                choicePrefab.weapName = tempWeap.wName;
                choicePrefab.weapImg.sprite = tempWeap.shopItemInfo.splashImg;
                choicePrefab.weapImg.gameObject.SetActive(true);
                choicePrefab.GetComponent<UIInteracableObjectVisuals>().SetNormal(false);
                choicePrefab.isSelected = false;
                choicePrefab.GetComponent<Button>().interactable = true;
                i++;
            }
            else
            {
                WeapChoicePrefab choicePrefab = weapOptions[i].GetComponent<WeapChoicePrefab>();
                choicePrefab.weapName = "";
                choicePrefab.GetComponent<WeapChoicePrefab>().weapImg.gameObject.SetActive(false);
                choicePrefab.GetComponent<Button>().interactable = false;
                choicePrefab.GetComponent<UIInteracableObjectVisuals>().SetNormal(false);
                choicePrefab.isSelected = false;
            }
            
            // if(tempWeap.currentEquippedSlots < tempWeap.maxSlots)
            // {
            //     //GameObject tempWeapChoicePrefab = Instantiate(weaponChoicePrefab);
            //     //tempWeapChoicePrefab.transform.SetParent(weapChoicePrefabContainer.transform);
 
                
            //     weapOptions[i].GetComponent<Button>().interactable = true;
            //     i++;
            //     //weapOptions.Add(tempWeapChoicePrefab);
            // }
        }
    }

    public void ClearOptions()
    {
        foreach(GameObject go in weapOptions)
        {
            //make the slots empty
            go.GetComponent<WeapChoicePrefab>().weapName = "";
            go.GetComponent<WeapChoicePrefab>().weapImg.gameObject.SetActive(false);
            go.GetComponent<Button>().interactable = false;
            go.GetComponent<UIInteracableObjectVisuals>().SetNormal(false);
            GetComponent<MenuItemSelectionManager>().menuItems[6].GetComponent<UIInteracableObjectVisuals>().SetRed(false);
            //GetComponent<MenuItemSelectionManager>().menuItems[6].GetComponent<Button>().interactable = false;
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
                //ws.GetComponent<UIInteracableObjectVisuals>().SetGreen(false);
                if(wcp.isSelected != true)
                {
                    //select it
                    Debug.Log(wcp.weapName + " not selected, selecting and setting green.");
                    wcp.isSelected = true;
                    //wcp.Select();
                    selectedWeap = wcp;
                    StartCoroutine(GetComponent<MenuItemSelectionManager>().SetSelectedAfterOneFrame(6));
                    //GetComponent<MenuItemSelectionManager>().menuItems[6].GetComponent<Button>().interactable = true;
                    GetComponent<MenuItemSelectionManager>().menuItems[6].GetComponent<UIInteracableObjectVisuals>().SetGreen(false);
                }
                else
                {
                    Debug.Log(wcp.weapName + " already selected, selecting apply button");
                    StartCoroutine(GetComponent<MenuItemSelectionManager>().SetSelectedAfterOneFrame(6));
                    //GetComponent<MenuItemSelectionManager>().menuItems[6].GetComponent<Button>().interactable = true;
                    GetComponent<MenuItemSelectionManager>().menuItems[6].GetComponent<UIInteracableObjectVisuals>().SetGreen(false);
                }
            }
            else
            {
                Debug.Log(wcp.weapName + " not the weap we are looking for, deSelecting and/or setting off bool to false.");
                wcp.isSelected = false;
                //wcp.GetComponent<UIInteracableObjectVisuals>().SetUnselected();
                //wcp.Deselect();
            }
        }

        ApplyGreenAndDeselectAll();
    }

    public void ApplyGreenAndDeselectAll()
    {
        if(selectedWeap != null)
        {
            foreach(GameObject ws in weapOptions)
            {
                WeapChoicePrefab wcp = ws.GetComponent<WeapChoicePrefab>();
                if(!wcp.isSelected)
                {
                    wcp.GetComponent<UIInteracableObjectVisuals>().SetNormal(false);
                }
            }

            selectedWeap.GetComponent<UIInteracableObjectVisuals>().SetGreen(false);
        }
    }

    //Apply Button calls this
    public void ApplySelectedWeapon()
    {
        if(selectedWeap != null)
        {
            relicToApply.ApplyRelic(player, selectedWeap.weapName);
            player.mainController.CloseWeapSelectEffect();
        }
        else
        {
            Debug.Log("Choose an upgrade first");
        }
    }
}