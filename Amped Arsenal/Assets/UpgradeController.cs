using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public MainMenuController mainController;
    public GameObject upgradeParentObj, upgradeSquarePrefab;
    private MainMenuController.BaseUpgrade tempUpgrade = new MainMenuController.BaseUpgrade();
    [SerializeField]
    public List<MainMenuController.BaseUpgrade> upgradeList;
    public List<GameObject> upgradePrefabs = new();
    
    [SerializeField]
    public List<UpgradeValues> upVals;
    public int[] upgradeCostValues = new int[5];

    public void Awake()
    {
        upgradeList ??= new List<MainMenuController.BaseUpgrade>();
        #region the code below equals this ^
        /*if(upgradeList == null)
        {
            upgradeList = new List<MainMenuController.BaseUpgrade>();
        }*/
        #endregion
    }

    public void InitUpgradeList()
    {
        //load values to tempUpgrade
        tempUpgrade.upgradeName = "Strength";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];


        //save it to player prefs so when you start a game it will load the value in the fight zone scene correctly
        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "HP";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "Armor";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "Speed";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Magnet";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Inflation";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
    }

    public void LoadUpgradeList()
    {
        int i = 0;
        foreach(MainMenuController.BaseUpgrade bu in upgradeList)
        {
            //GameObject temp;
            //temp = Instantiate(upgradeSquarePrefab, upgradeParentObj.transform);
            //set name and level of the prefab
            if(!bu.IsMaxLevel())
            {
                upgradePrefabs[i].GetComponent<BaseUpgradeSquare>().SetUpgradeVisuals(bu.upgradeName, bu.UpgradeLevel, upgradeCostValues[bu.UpgradeLevel]);
            }
            else
            {
                upgradePrefabs[i].GetComponent<BaseUpgradeSquare>().SetUpgradeVisuals(bu.upgradeName, bu.UpgradeLevel, -1);
            }
            
            i++;
        }
    }

    public void ChooseUpgrade(string upName)
    {
        MainMenuController.BaseUpgrade tempUpgrade = new();
        tempUpgrade.upgradeName = "";
        tempUpgrade.UpgradeLevel = 0;

        //Get the upgrade from the upgrade list to update the data to be saved
        foreach(MainMenuController.BaseUpgrade bu in upgradeList)
        {
            if (bu.upgradeName == upName)
            {
                tempUpgrade = bu;
            }
        }

        //if not max level then you can see if you can buy
        if(!tempUpgrade.IsMaxLevel())
        {
            if(mainController.PlayerGold < upgradeCostValues[tempUpgrade.UpgradeLevel])
            {
                //cant buy
                Debug.Log("Cant buy");
            }
            else
            {
                //subtract money
                mainController.PlayerGold -= upgradeCostValues[tempUpgrade.UpgradeLevel];
                
                //up the level
                tempUpgrade.UpgradeLevel++;

                //update the values to be loaded into the fight zone
                mainController.UpdatePlayerPrefs();

                //Get the visual gameobject to update
                foreach(GameObject go in upgradePrefabs)
                {
                    if(tempUpgrade.upgradeName == go.GetComponent<BaseUpgradeSquare>().baseUpgradeName)
                    {
                        //update price on upgrade
                        BaseUpgradeSquare bus = go.GetComponent<BaseUpgradeSquare>();
                        if(tempUpgrade.IsMaxLevel())
                        {
                            bus.SetUpgradeVisuals(tempUpgrade.UpgradeLevel, -1);
                        }
                        else
                        {
                            bus.SetUpgradeVisuals(tempUpgrade.UpgradeLevel, upgradeCostValues[tempUpgrade.UpgradeLevel]);
                        }
                         
                        bus.UpdateBar(true);              
                    }
                }  

                //save upgrade
                mainController.SaveUpgradeValues();
            }
        }
        else
        {
            Debug.Log("Upgrade is Max level");
        }
    }

    public void SaveStuffButton()
    {
        mainController.SaveUpgradeValues();
    }

    
}

[System.Serializable]
public class UpgradeValues
{
    [SerializeField]
    public string upName;
    [SerializeField]
    public int[] upValues;
}
